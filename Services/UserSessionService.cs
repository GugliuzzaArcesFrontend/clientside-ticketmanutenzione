using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace clientside_ticketmanutenzione.Services;

public class UserSessionService : IDisposable
{   
    private readonly AuthenticationStateProvider _authStateProvider;
    public UserSessionService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
        _authStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }
    public string? UserId { get; private set; }
    public string? Nominativo { get; private set; }
    public string? Email { get; private set; }
    public string? Ruolo { get; private set; }
    public string? SessionId { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public event Action? OnChange;

    //public void SetUser(ClaimsPrincipal user)
    //{
    //    IsAuthenticated = user.Identity?.IsAuthenticated ?? false;

    //    UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //    Nominativo = user.FindFirst(ClaimTypes.Name)?.Value;
    //    Email=user.FindFirst(ClaimTypes.Email)?.Value;
    //    SessionId = user.FindFirst("sid")?.Value;
    //    Ruolo = user.FindFirst(ClaimTypes.Role)?.Value;
    //}
    //public void ClearUser()
    //{
    //    IsAuthenticated = false;

    //    UserId=null;
    //    Nominativo=null;
    //    Email=null;
    //    SessionId=null;
    //    Ruolo=null;
    //}

    public void SetUser(ClaimsPrincipal user)
    {
        IsAuthenticated = user.Identity?.IsAuthenticated ?? false;
        UserId = IsAuthenticated ? user.FindFirst(ClaimTypes.NameIdentifier)?.Value:null;
        Nominativo= IsAuthenticated ? user.FindFirst(ClaimTypes.Name)?.Value : null;
        Email = IsAuthenticated ? user.FindFirst(ClaimTypes.Email)?.Value : null;
        Ruolo = IsAuthenticated ? user.FindFirst(ClaimTypes.Role)?.Value:null;
        OnChange?.Invoke();
    }
    public void Dispose()
    {
        _authStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
    }
    //metodi per il mantenimento aggiornato dello user in sessione
    public async Task InitializeUserAsync() 
    {
        var authState= await _authStateProvider.GetAuthenticationStateAsync();
        SetUser(authState.User);
    }
    private async Task UpdateUserAsync(Task<AuthenticationState> task)
    {
        var authState = await task;
        SetUser(authState.User);
    }
    private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        _ = UpdateUserAsync(task);
    }
}
