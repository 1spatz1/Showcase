using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Showcase.Domain.Entities;

namespace Showcase.Infrastructure.Persistence.EntityConfiguration;

public class BoardPositionEntityConfiguration : IEntityTypeConfiguration<BoardPosition>
{
    public void Configure(EntityTypeBuilder<BoardPosition> builder)
    {
        builder
            .HasOne<Game>()
            .WithMany(g => g.Board)
            .HasForeignKey(bp => bp.GameId)
            .IsRequired();
    }
}