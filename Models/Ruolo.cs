
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_test1.Models
{
    [Table("ruoli",Schema ="dbo")]
    public class Ruolo
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }

        
        [Required]
        [Column("ruolo")]
        [MaxLength(100)]
        public string NomeRuolo { get; set; } = string.Empty;

        public ICollection<Utenza> Utenza { get; set; } = new List<Utenza>();
    }
}