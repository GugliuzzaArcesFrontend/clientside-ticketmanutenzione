using System.ComponentModel.DataAnnotations;

namespace clientside_ticketmanutenzione.Dtos
{
    public class LoginDto
    {
        [Required, 
            EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required,
            ] //dovrei creare una regex per caratteri non consentiti
        public string Password { get; set; } = string.Empty;
    }
}
