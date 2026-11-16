
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_test1.Models
{
    [Table("stati",Schema ="dbo")] 
    public class Stato
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("stato")]
        [Required]
        public string NomeStato { get; set; } = string.Empty;

        public ICollection<Segnalazione> Segnalazione { get; set; } = new List<Segnalazione>();
    }
}
