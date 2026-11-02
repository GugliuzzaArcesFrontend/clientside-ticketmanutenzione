using clientside_ticketmanutenzione.Dtos;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Serilog;
namespace clientside_ticketmanutenzione.Services;

public class AuthService(IJSRuntime _js)
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
            if (result!.Success)
            {
                return result;
            }
            return null;            
        }
        catch (JSException ex)
        {
            Log.Debug("{1}", ex.Message);
            return null;
        }
        finally 
        {
            
        }
    }
    public async Task<LogoutResult?> HandleLogout()
    {
        try
        {
            var result = await _js.InvokeAsync<LogoutResult?>("authLogout");
            if (result is not null && result.Success)
            {
                return result;
            }
            return null;
        }
        catch (JSException ex)
        {
            Log.Debug("Fallito il logout");
            return null;
        }
        finally 
        {
        
        }
    }
}
