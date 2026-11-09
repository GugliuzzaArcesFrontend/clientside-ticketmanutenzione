namespace clientside_ticketmanutenzione.Handlers;

using clientside_ticketmanutenzione.Models;
using clientside_ticketmanutenzione.Services;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class AuthTokenHandler (AuthSessionManager _sessionManager,IHttpClientFactory _httpClientFactory) : DelegatingHandler
{
    public static readonly HttpRequestOptionsKey<string> SessionIdKey = new("SessionId");
    private static readonly TimeSpan RefreshThreshold = TimeSpan.FromSeconds(60);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Options.TryGetValue(SessionIdKey, out var sessionId)
            || string.IsNullOrWhiteSpace(sessionId))
        {
            return Unauthorized(request);
        }

        var refreshLock = _sessionManager.GetRefreshLock(sessionId);

        AuthSession? session;

        await refreshLock.WaitAsync(cancellationToken);

        try
        {
            session = _sessionManager.GetSession(sessionId);

            if (session is null)
                return Unauthorized(request);

            // Verifica della scadenza imminente
            if (session.AccessTokenExpiration <=
                DateTime.UtcNow.Add(RefreshThreshold))
            {
                var refreshed = await RefreshSessionAsync(
                    sessionId,
                    session,
                    cancellationToken);

                if (!refreshed)
                    return Unauthorized(request);
            }

            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    session.TokenType,
                    session.AccessToken);
        }
        finally
        {
            refreshLock.Release();
        }
        //if (request.Options.TryGetValue(SessionIdKey, out var sessionId))
        //{            
        //    var session = _sessionManager.GetSession(sessionId);
        //    if (session is not null)
        //    {
        //        request.Headers.Authorization =
        //        new AuthenticationHeaderValue(
        //            session.TokenType,
        //            session.AccessToken);
        //        return await base.SendAsync(request, cancellationToken);
        //    }        
        //}
        // Conserva il contenuto per poter ripetere la richiesta.
        if (request.Content is not null)
        {
            await request.Content.LoadIntoBufferAsync(
                cancellationToken);
        }

        var accessTokenUsed = session.AccessToken;

        var response = await base.SendAsync(
            request,
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        // Prima richiesta fallita: liberiamo la risposta.
        response.Dispose();

        await refreshLock.WaitAsync(cancellationToken);
        try
        {
            session = _sessionManager.GetSession(sessionId);

            if (session is null)
                return Unauthorized(request);

            // Un'altra richiesta potrebbe aver già rinnovato il token.
            if (session.AccessToken == accessTokenUsed)
            {
                var refreshed = await RefreshSessionAsync(
                    sessionId,
                    session,
                    cancellationToken);

                if (!refreshed)
                    return Unauthorized(request);
            }

            // Creazione di una nuova richiesta HTTP.
            using var retryRequest = await CloneRequestAsync(
                request,
                cancellationToken);

            retryRequest.Headers.Authorization =
                new AuthenticationHeaderValue(
                    session.TokenType,
                    session.AccessToken);

            // Un solo nuovo tentativo.
            return await base.SendAsync(
                retryRequest,
                cancellationToken);
        }
        finally
        {
            refreshLock.Release();
        }
    

        return await base.SendAsync(request, cancellationToken);


    }

    private async Task<bool> RefreshSessionAsync(
        string sessionId,
        AuthSession session,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("apiLogin");

        var response = await client.PostAsJsonAsync(
            "auth/refresh",
            new { RefreshToken = session.RefreshToken },
            cancellationToken);

        if (response.StatusCode is HttpStatusCode.Unauthorized
            or HttpStatusCode.Forbidden)
        {
            
            return false;
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<RefreshResponse>(
                cancellationToken: cancellationToken);

        if (result is null ||
            string.IsNullOrWhiteSpace(result.AccessToken) ||
            string.IsNullOrWhiteSpace(result.RefreshToken) ||
            result.ExpiresIn <= 0)
        {
            throw new InvalidOperationException(
                "Risposta di refresh non valida.");
        }

        session.AccessToken = result.AccessToken;
        session.RefreshToken = result.RefreshToken;

        session.AccessTokenExpiration =
            DateTime.UtcNow.AddSeconds(result.ExpiresIn);

        session.RefreshTokenExpirationDate =
            result.RefreshExpiresAt;

        return true;
    }
    private static HttpResponseMessage Unauthorized(
       HttpRequestMessage request)
    {
        return new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            RequestMessage = request
        };
    }
    private static async Task<HttpRequestMessage> CloneRequestAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var clone = new HttpRequestMessage(
            request.Method,
            request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(
                header.Key,
                header.Value);
        }

        foreach (var option in request.Options)
        {
            clone.Options.Set(
                new HttpRequestOptionsKey<object?>(option.Key),
                option.Value);
        }

        if (request.Content is not null)
        {
            var bytes = await request.Content.ReadAsByteArrayAsync(
                cancellationToken);

            var content = new ByteArrayContent(bytes);

            foreach (var header in request.Content.Headers)
            {
                content.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }

            clone.Content = content;
        }

        return clone;
    }
    private sealed class RefreshResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public DateTime RefreshExpiresAt { get; set; }
    }
}
