namespace clientside_ticketmanutenzione.Dtos;

public class LoginResponseDto
{
    public UtenzaDto Utenza { get; set; } = new();
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public DateTime RefreshExpiresAt { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class UtenzaDto
{
    public int Id { get; set; }
    public int IdRuolo { get; set; }
    public string Cognome { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string CodiceFiscale { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public object? Ruolo { get; set; }
}
