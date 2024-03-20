using System.ComponentModel.DataAnnotations;
using Showcase.Domain.Entities;

namespace Showcase.Domain.Identity;

public class UserTotpSecret
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string TotpSecret { get; set; }
    public ICollection<string>? BackupCodes { get; set; }
}