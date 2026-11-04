using System.ComponentModel.DataAnnotations;

namespace clientside_ticketmanutenzione.Dtos
{
    public class LoginDto
    {
        [Required, 
            EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required,
            StringLength(16, MinimumLength = 8, ErrorMessage = "La password deve essere compresa fra 8 e 16 caratteri")
            ] //dovrei creare una regex per caratteri non consentiti
        public string Password { get; set; } = string.Empty;
    }
}
