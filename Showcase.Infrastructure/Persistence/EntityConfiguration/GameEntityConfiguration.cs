using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Showcase.Domain.Entities;

namespace Showcase.Infrastructure.Persistence.EntityConfiguration;

public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder
            .HasOne<ApplicationUser>()
            .WithMany(u => u.PlayerOneGames)
            .HasForeignKey(g => g.PlayerOneId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<ApplicationUser>()
            .WithMany(u => u.PlayerTwoGames)
            .HasForeignKey(g => g.PlayerTwoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany<BoardPosition>(g => g.Board)
            .WithOne()
            .HasForeignKey(bp => bp.GameId)
            .IsRequired();
    }
}