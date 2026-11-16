namespace api_test1.Dtos;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class PostAzioneDto
{
    [Required]
    [Column("id_segnalazione")]
    public int IdSegnalazione { get; set; }

    [Column("descrizione")]
    [MaxLength(255)]
    public string? Descrizione { get; set; } = string.Empty;

    [Required]
    [Column("azione")]
    [MaxLength(100)]
    public string NomeAzione { get; set; } = string.Empty;

    [Required]
    [Column("data")]
    public DateOnly DataAzione { get; set; }

    [Required]
    public int IdStato { get; set; }
}
