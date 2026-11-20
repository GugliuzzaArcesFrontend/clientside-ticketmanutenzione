using System.ComponentModel.DataAnnotations;

namespace clientside_ticketmanutenzione.Dtos;

public class InputAzioneDto
{
    public int? Id { get; set; }

    [Required]
    public int IdSegnalazione { get; set; }

    [Required]
    public int IdStato { get; set; }

    [Required]
    public string NomeAzione { get; set; } = string.Empty;

    [Required]
    public string Descrizione { get; set; } = string.Empty;

    [Required]
    public DateOnly DataAzione { get; set; }

    public DateOnly? DataModifica { get; set; }

    public DateOnly? DataChiusura { get; set; }
}