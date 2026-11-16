
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_test1.Models
{
    [Table("segnalazioni",Schema ="dbo")]
    public class Segnalazione
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("id_utenza")]
        [Required]
        public int IdUtenza { get; set; }

        [Column("id_stato")]
        [Required]
        public int IdStato { get; set; }

        [Column("id_tipologia")]
        [Required]
        public int IdTipologia { get; set; }

        [Required,
            Column("luogo"),
            MaxLength(100)]
        public string Luogo { get; set; } = string.Empty;

        [Column("data")]
        [Required]
        public DateTime DataSegnalazione {  get; set; }

        [Column("data_chiusura")]
        public DateTime? DataChiusura { get; set; }
        [Column("descrizione")]
        [MaxLength(255)]
        public string? Descrizione { get; set; }

        [ForeignKey(nameof(IdUtenza))]
        public Utenza Utenza { get; set; } = null!;

        [ForeignKey(nameof(IdStato))]
        public Stato Stato { get; set; } = null!;

        [ForeignKey(nameof(IdTipologia))]
        public Tipologia Tipologia { get; set; } = null!;

        public ICollection<Azione> Azione { get; set; } = new List<Azione>();
        
        public ICollection<Allegato> Allegato { get; set; } = new List<Allegato>();
    }
}
