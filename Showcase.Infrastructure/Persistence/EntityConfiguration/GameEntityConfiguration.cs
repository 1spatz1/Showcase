using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Showcase.Domain.Entities;

namespace Showcase.Infrastructure.Persistence.EntityConfiguration;

public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder
            .HasOne<ApplicationUser>(g => g.PlayerOne)
            .WithMany()
            .HasForeignKey(g => g.PlayerOneId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<ApplicationUser>(g => g.PlayerTwo)
            .WithMany()
            .HasForeignKey(g => g.PlayerTwoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}