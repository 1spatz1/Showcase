using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Showcase.Application.Common.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}