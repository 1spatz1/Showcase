using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;

namespace Showcase.Infrastructure.Persistence.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        builder
            .HasMany(x => x.PlayerOneGames)
            .WithOne() 
            .HasForeignKey(x => x.PlayerOneId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.PlayerTwoGames)
            .WithOne() 
            .HasForeignKey(x => x.PlayerTwoId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.UserTotpSecret)
            .WithOne(x => x.User)
            .HasForeignKey<UserTotpSecret>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}