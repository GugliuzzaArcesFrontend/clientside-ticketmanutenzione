namespace clientside_ticketmanutenzione.Endpoints;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using System.Net.Http.Json;
using System.Security.Claims;
using Serilog;

using clientside_ticketmanutenzione.Dtos;
using clientside_ticketmanutenzione.Models;
using clientside_ticketmanutenzione.Services;
using clientside_ticketmanutenzione.Utilities;

public static class AuthEndpoints 
{

    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/auth");

        group.MapPost("/login", async (
            LoginDto request,
            IHttpClientFactory httpClientFactory,
            AuthSessionManager sessionManager,
            HttpContext context) =>
        {
            var client = httpClientFactory.CreateClient("api_test1");

            var response = await client.PostAsJsonAsync("/api/auth/login", request);
            Log.Debug($"response?.Content.ToString()");
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (loginResponse is null) return Results.Problem("Risposta non valida dal backend.");
                string ruolo = loginResponse.Utenza.IdRuolo switch
                {
                    1 => "Amministratore",
                    2=> "Operatore",
                    _ => "Manutentore"
                };
                var session = new AuthSession
                {
                    UserID = loginResponse.Utenza.Id.ToString(),
                    Cognome = loginResponse.Utenza.Cognome,
                    Nome = loginResponse.Utenza.Nome,
                    Email = loginResponse.Utenza.Email,
                    Ruolo = ruolo,
                    AccessToken = loginResponse.AccessToken,
                    TokenType = loginResponse.TokenType,
                    AccessTokenExpiration = DateTime.UtcNow.AddSeconds(
                        loginResponse.ExpiresIn),
                    RefreshToken = loginResponse.RefreshToken,
                    RefreshTokenExpirationDate = loginResponse.RefreshExpiresAt
                };
                if (loginResponse is null 
                    || loginResponse.Utenza.Id <= 0 
                    || string.IsNullOrWhiteSpace(loginResponse.AccessToken) 
                    || string.IsNullOrWhiteSpace(loginResponse.RefreshToken) 
                    || loginResponse.ExpiresIn <= 0)
                {
                    return Results.Problem(
                        "Risposta non valida dal backend.",
                        statusCode: StatusCodes.Status502BadGateway);
                }
                var sessionId = sessionManager.CreateSession(session);

                var claims = new List<Claim>
                {
                    new ("sid", sessionId),
                    new (ClaimTypes.NameIdentifier,session.UserID),
                    new (ClaimTypes.Name,$"{session.Nome} {session.Cognome}"),
                    new (ClaimTypes.Email,session.Email),
                    new (ClaimTypes.Role,session.Ruolo)
                };
                var identity = new ClaimsIdentity(claims, 
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                try
                {
                    await context.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);
                }
                catch
                {
                    sessionManager.RemoveSession(sessionId); 
                    throw;
                }
                return Results.Ok(new
                {
                    authenticated = true,
                    message = loginResponse.Message
                });
            }
            return ApiResponseHelper.HandleError(response.StatusCode);
        })
        .AllowAnonymous();

        group.MapPost("/logout", async (
            HttpContext context,
            AuthSessionManager sessionManager) =>
            {
                var sessionId = context.User.FindFirst("sid")?.Value;
                if (!string.IsNullOrWhiteSpace(sessionId))
                {
                    sessionManager.RemoveSession(sessionId);
                }
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Ok(new {
                    authenticated = false,
                    message = "Logout effettuato correttamente."});
            })
            .RequireAuthorization();

        group.MapGet("/me", (HttpContext context) =>
        {
            var user = context.User;

            return Results.Ok(new
            {
                UserID = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                NomeCompleto = user.FindFirst(ClaimTypes.Name)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                Ruolo = user.FindFirst(ClaimTypes.Role)?.Value
            });
        })
        .RequireAuthorization();

        //group.MapPost("/sessionrefresh", (HttpContext context, AuthSessionManager authSessionManager) => 
        //{ 
        //});
        return endpoints;
    }
}