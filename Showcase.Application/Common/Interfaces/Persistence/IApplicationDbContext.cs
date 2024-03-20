using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;

namespace Showcase.Application.Common.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Game> Games { get; }
    DbSet<BoardPosition> BoardPositions { get; }
    DbSet<ApplicationUser> Users { get; }
    DbSet<UserTotpSecret> UserTotpSecrets { get; }
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}