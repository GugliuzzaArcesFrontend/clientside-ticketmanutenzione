using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

using clientside_ticketmanutenzione.Handlers;

namespace clientside_ticketmanutenzione.Services;

public class AuthenticatedHttpService(
    IHttpClientFactory httpClientFactory,
    AuthenticationStateProvider authenticationStateProvider,
    IJSRuntime jsRuntime,
    NavigationManager navigation)
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

        var response = await client.SendAsync(request, cancellationToken);
        return await HandleResponseAsync(response);
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

        return await HandleResponseAsync(response);
    }

    public async Task<HttpResponseMessage> SendAsync<T>(
        HttpClient client,
        HttpMethod method,
        string uri,
        T body,
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
        request.Content = JsonContent.Create(body);

        var response = await client.SendAsync(
            request,
            cancellationToken);

        return await HandleResponseAsync(response);
    }

    private async Task<HttpResponseMessage> HandleResponseAsync(
        HttpResponseMessage response)
    {
        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
            return response;

        try
        {
            await jsRuntime.InvokeVoidAsync("authLogout");
        }
        catch (JSException)
        {
            // Implementare gestione errori js
        }

        navigation.NavigateTo("/login", forceLoad: true);

        return response;
    }
}