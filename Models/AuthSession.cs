namespace clientside_ticketmanutenzione.Models;

public sealed class AuthSession
{   
    //dati dei token
    public required string AccessToken { get; set; }
    public required string TokenType { get; set; }
    public required DateTime AccessTokenExpiration {  get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefreshTokenExpirationDate { get; set; }
    //dati utente
    public required string UserID { get; set; }
    public required string Nome { get; set; }
    public required string Cognome { get; set; }
    public required string Email { get; set; }
    public required string Ruolo { get; set; }
}