using System.Net.Http;

using Microsoft.AspNetCore.Components.Authorization;

using clientside_ticketmanutenzione.Handlers;

namespace clientside_ticketmanutenzione.Services;

public class AuthenticatedHttpService(
    IHttpClientFactory httpClientFactory,
    AuthenticationStateProvider authenticationStateProvider)
{
    public async Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string uri,
        CancellationToken cancellationToken = default)
    {
        var authState = await authenticationStateProvider
            .GetAuthenticationStateAsync();

        var sessionId = authState.User.FindFirst("sid")?.Value;

        if (string.IsNullOrWhiteSpace(sessionId))
            throw new UnauthorizedAccessException("Sessione non valida.");

        var request = new HttpRequestMessage(method, uri);

        request.Options.Set(
            AuthTokenHandler.SessionIdKey,
            sessionId);

        var client = httpClientFactory.CreateClient("api_test1");

        return await client.SendAsync(request, cancellationToken);
    }

    public async Task<HttpResponseMessage> SendAsync(
        HttpClient client,
        HttpMethod method,
        string uri,
        CancellationToken cancellationToken = default)
    {
        var authState = await authenticationStateProvider
            .GetAuthenticationStateAsync();

        var sessionId = authState.User.FindFirst("sid")?.Value;

        if (string.IsNullOrWhiteSpace(sessionId))
            throw new UnauthorizedAccessException("Sessione non valida.");

        using var request = new HttpRequestMessage(method, uri);

        request.Options.Set(
            AuthTokenHandler.SessionIdKey,
            sessionId);

        var response = await client.SendAsync(
            request,
            cancellationToken);

        return response;
    }
}
