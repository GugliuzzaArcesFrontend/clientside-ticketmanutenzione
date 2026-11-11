using clientside_ticketmanutenzione.Dtos;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Serilog;

namespace clientside_ticketmanutenzione.Services;

public class AuthService(IJSRuntime _js,  NavigationManager _navigation)
{
    public sealed class LoginResult
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
    public sealed class LogoutResult
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
    public async Task<LoginResult?> HandleLogin(LoginDto dto)
    {
        try
        {
            var result = await _js.InvokeAsync<LoginResult>("authLogin", dto);
            if (result is not null && result.Success)
            {
                _navigation.Refresh(forceReload:true);
            }
            return result;            
        }
        catch (JSException ex)
        {
            Log.Debug("{1}", ex.Message);
            return null;
        }
        catch(Exception ex)
        {
            Log.Debug("Eccezione in AuthService.HandleLogin\n Messaggio:{ex.Message}", ex.Message);
            return null;
        }
    }
    public async Task<LogoutResult?> HandleLogout()
    {
        try
        {
            var result = await _js.InvokeAsync<LogoutResult?>("authLogout");
            if (result is not null && result.Success)
            {
                _navigation.NavigateTo("/", forceLoad: true);
            }
            return result;
        }
        catch (JSException ex)
        {
            Log.Debug("Fallito il logout, {aaa}",ex.Message);
            return null;
        }
    }
}
