namespace clientside_ticketmanutenzione.Models;

public class AuthSession
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }

    public required DateTimeOffset RefreshTokenExpirationDate { get; set; }

    public required string UserID { get; set; }
}