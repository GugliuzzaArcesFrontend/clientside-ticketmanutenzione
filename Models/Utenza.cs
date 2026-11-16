
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_test1.Models
{
    [Table("utenze")]
    public class Utenza
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("id_ruolo")]
        [Range(1,4, ErrorMessage ="Ruolo non valido")]
        public int IdRuolo { get; set; }
        
        [Required]
        [Column("cognome")]
        [RegularExpression(@"^[a-zA-ZÀ-ÖØ-öø-ÿ'\s]+$")]
        [MaxLength(100)]
        public string Cognome { get; set; } = string.Empty;
        
        [Required]
        [Column("nome")]
        [RegularExpression(@"^[a-zA-ZÀ-ÖØ-öø-ÿ'\s]+$")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
        
        [Column("codice_fiscale")]
        [Required]
        [MaxLength(16)]
        [RegularExpression(@"(?i)^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[ABCDEHLMPRST][0-9]{3}[A-Z]$",
            ErrorMessage = "Inserire un codice fiscale valido")]
        public string CodiceFiscale { get; set; } = string.Empty;

        [Column("email")]
        [MaxLength(255)]
        //[RegularExpression(@"(i)^[a-z0-9-._~]+@[a-z0-9-_~]\.[a-z0-9]{2,4}$")]
        [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido.")]
        [Required]
        public string Email { get; set; } = string.Empty;

        [ForeignKey(nameof(IdRuolo))]
        public Ruolo? Ruolo { get; set; }

        public ICollection<Azione> Azioni { get; set; } = new HashSet<Azione>();
        
        public ICollection<Segnalazione> Segnalazione { get; set; } = new List<Segnalazione>();
    }
}