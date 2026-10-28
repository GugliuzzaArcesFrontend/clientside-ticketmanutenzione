using System.ComponentModel.DataAnnotations;

namespace clientside_ticketmanutenzione.Dtos
{
    public class InputUtenzaDto
    {
        public int? Id { get; set; }
        
        [Required]
        public int IdRuolo { get; set; }

        [Required,
            RegularExpression(@"^[a-zA-ZÀ-ÖØ-öø-ÿ'\s]+?")]
        public string Cognome { get; set; } = string.Empty;

        [Required,
            RegularExpression(@"^[a-zA-ZÀ-ÖØ-öø-ÿ'\s]+$")]
        public string Nome { get; set; } = string.Empty;

        [Required,
            StringLength(16, MinimumLength = 16),
            RegularExpression(@"(?i)^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[ABCDEHLMPRST][0-9]{3}[A-Z]$")]
        public string CodiceFiscale { get; set; } = string.Empty;
        
        [Required,
            EmailAddress]
        public string Email { get; set; } = string.Empty;


    }
}
