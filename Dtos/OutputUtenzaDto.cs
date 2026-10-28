namespace clientside_ticketmanutenzione.Dtos
{
    public class OutputUtenzaDto
    {
        public int Id { get; set; }
        public string Cognome { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string CodiceFiscale { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Ruolo { get; set; } = string.Empty;
    }
}
