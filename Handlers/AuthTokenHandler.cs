namespace clientside_ticketmanutenzione.Handlers;

using System.Net;
using System.Net.Http.Headers;

using clientside_ticketmanutenzione.Services;

public class AuthTokenHandler (AuthSessionManager _sessionManager) : DelegatingHandler
{
    public static readonly HttpRequestOptionsKey<string> SessionIdKey = new("SessionId");
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Options.TryGetValue(SessionIdKey, out var sessionId))
        {            
            var session = _sessionManager.GetSession(sessionId);
            if (session is not null)
            {
                request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    session.TokenType,
                    session.AccessToken);
                return await base.SendAsync(request, cancellationToken);
            }        
        }
        return new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            RequestMessage = request
        };
    }
}
