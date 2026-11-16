
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_test1.Models
{
    [Table("allegati", Schema ="dbo")]
    
    public class Allegato
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("id_segnalazione")]
        public int IdSegnalazione { get; set; }

        [Required]
        [Column ("fileurl")]
        [MaxLength(100)]
        //ci devo sbattere una regex relativa alla fonte?
        public string FileUrl { get; set; } = string.Empty;

        [Column ("descrizione")]
        [MaxLength(255)]
        public string? Descrizione { get; set; }

        [ForeignKey(nameof(IdSegnalazione))]
        public Segnalazione Segnalazione { get; set; } = null!;
    }
}
