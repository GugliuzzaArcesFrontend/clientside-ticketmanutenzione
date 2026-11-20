
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_test1.Models;


public class Azione
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("id_segnalazione")]
    public int IdSegnalazione { get; set; }

    [Column("descrizione")]
    [MaxLength(255)]
    public string? Descrizione { get; set; } = string.Empty;

    [Required]
    [Column("azione")]
    [MaxLength(100)]
    public string NomeAzione { get; set; } =string.Empty;

    [Required]
    [Column("data")]
    public DateOnly DataAzione { get; set; }

    [Column("data_chiusura")]
    public DateOnly? DataChiusura { get; set; }

    [Column("data_modifica")]
    public DateOnly? DataModifica { get; set; }

    [Required,
        Column("id_stato")]
    public int IdStato { get; set; }

    [ForeignKey(nameof(IdSegnalazione))]
    public Segnalazione Segnalazione { get; set; } = null!;

    [ForeignKey(nameof(IdStato))]
    public Stato Stato { get; set; }= null!;
    public ICollection<Utenza> Utenze { get; set; } = new HashSet<Utenza>();
}