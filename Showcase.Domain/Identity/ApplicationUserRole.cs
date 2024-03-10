using Microsoft.AspNetCore.Identity;
using Showcase.Domain.Entities;

namespace Showcase.Domain.Identity;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public ApplicationUser User { get; set; }
    public ApplicationRole Role { get; set; }
}