using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;

namespace Showcase.Api;

public class DbSeeder
{
    private const string AdminShowcaseInternal = "admin@showcase.internal";

    public static async Task SeedDbAsync(IApplicationDbContext context, UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        // Seed default roles
        List<ApplicationRole> roles = new()
        {
            new ApplicationRole
            {
                Name = IdentityNames.Roles.Administrator
            },
            new ApplicationRole
            {
                Name = IdentityNames.Roles.Member
            }
        };
        if (!await roleManager.Roles.AnyAsync())
        {
            foreach (ApplicationRole role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }
        
        // Seed default admin user
        if (!await userManager.Users.AnyAsync())
        {
            ApplicationUser? user = new()
            {
                UserName = "super-admin",
                Email = AdminShowcaseInternal
            };
                IdentityResult result = await userManager.CreateAsync(user, "ShowcaseAPI@AdminPassword_ChangeMe-123");
            if (result.Succeeded)
            {
                user = await userManager.FindByEmailAsync(AdminShowcaseInternal);
                // Seed the default roles onto the admin user
                await userManager.AddToRoleAsync(user, "Administrator");
            }
        }
    }
}