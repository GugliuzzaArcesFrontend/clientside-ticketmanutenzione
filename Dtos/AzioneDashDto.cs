using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clientside_ticketmanutenzione.Dtos;

public class AzioneDashDto
{
    public int Id { get; set; }
    public int IdSegnalazione { get; set; }
    public string? Descrizione { get; set; } = string.Empty;
    public string NomeAzione { get; set; } = string.Empty;
    public DateOnly DataAzione { get; set; }
    public DateOnly? DataChiusura { get; set; }
    public int IdStato { get; set; }
    public string DescrizioneSegnalazione { get; set; } = string.Empty;
}