
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_test1.Models
{
    [Table("tipologie",Schema ="dbo")]
    
    public class Tipologia
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("tipologia")]
        [Required]
        [MaxLength(100)]
        public string NomeTipologia { get; set; } = string.Empty;
    }
}
