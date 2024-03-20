using System.ComponentModel.DataAnnotations;

namespace Showcase.Domain.Entities;

public class UserTotpSecret
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string TotpSecret { get; set; }
    public ICollection<string>? BackupCodes { get; set; }
}