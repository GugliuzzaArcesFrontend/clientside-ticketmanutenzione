using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_test1.Models;

[Table("refresh_tokens", Schema = "dbo")]
public class RefreshToken
{
    [Key,
        Column("id"),
        DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required,
        Column("id_utenza")]
    public int IdUtenza { get; set; }
    [Required,
        Column("token_hash"),
        MaxLength(64)]
    public string TokenHash { get; set; } = string.Empty;

    [Required,
        Column("family_id")]
    public Guid FamilyId { get; set; }

    [Required,
        Column("expires_at")]
    public DateTime ExpiresAtUtc { get; set; }

    [Column("used_at")]
    public DateTime? UsedAtUtc { get; set; }

    [Column("revoked_at")]
    public DateTime? RevokedAtUtc { get; set; }

    [ForeignKey(nameof(IdUtenza))]
    public Utenza Utenza { get; set; } = null!;
}