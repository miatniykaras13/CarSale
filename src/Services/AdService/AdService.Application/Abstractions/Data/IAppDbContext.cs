using AdService.Domain.Aggregates;
using AdService.Domain.Entities;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Abstractions.Data;

public interface IAppDbContext
{
    DbSet<Ad> Ads { get; }

    DbSet<Comment> Comments { get; }

    DbSet<CarOption> CarOptions { get; }

    Task<T> ReadSnapshotAsync<T>(Func<CancellationToken, Task<T>> read, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
