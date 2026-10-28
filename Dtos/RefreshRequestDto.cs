using System.ComponentModel.DataAnnotations;

namespace clientside_ticketmanutenzione.Dtos;

public class RefreshRequestDto
{
    [Required]
    [MaxLength(200)]
    public string RefreshToken { get; set; } = string.Empty;
}