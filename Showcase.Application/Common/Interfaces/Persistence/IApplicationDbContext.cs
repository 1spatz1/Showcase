using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Showcase.Application.Common.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Game> Games { get;  }
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}