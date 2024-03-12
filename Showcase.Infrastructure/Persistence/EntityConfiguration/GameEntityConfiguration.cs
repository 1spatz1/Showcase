using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Showcase.Domain.Entities;

namespace Showcase.Infrastructure.Persistence.EntityConfiguration;

public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder
            .HasOne(x => x.PlayerOne)
            .WithMany(x => x.PlayerOneGames)
            .HasForeignKey(x => x.PlayerOneId);

        builder
            .HasOne(x => x.PlayerTwo)
            .WithMany(x => x.PlayerTwoGames)
            .HasForeignKey(x => x.PlayerTwoId);
    }
}