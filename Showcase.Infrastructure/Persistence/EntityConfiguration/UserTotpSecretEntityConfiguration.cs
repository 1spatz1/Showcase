using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;

namespace Showcase.Infrastructure.Persistence.EntityConfiguration;

public class UserTotpSecretEntityConfiguration : IEntityTypeConfiguration<UserTotpSecret>
{
    public void Configure(EntityTypeBuilder<UserTotpSecret> builder)
    {
        builder
            .HasOne<ApplicationUser>(x => x.User)
            .WithOne(x => x.UserTotpSecret)           
            .HasForeignKey<UserTotpSecret>(x => x.UserId);
    }
}