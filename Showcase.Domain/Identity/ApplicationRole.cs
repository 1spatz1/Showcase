using Microsoft.AspNetCore.Identity;

namespace Showcase.Domain.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public ICollection<ApplicationUserRole> UserRoles { get; set; }
}