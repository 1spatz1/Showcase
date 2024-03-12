using Microsoft.AspNetCore.Identity;
using Showcase.Domain.Identity;

namespace Showcase.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<ApplicationUserRole> UserRoles { get; set; }
    public ICollection<Game> PlayerOneGames { get; set; }
    public ICollection<Game> PlayerTwoGames { get; set; }}