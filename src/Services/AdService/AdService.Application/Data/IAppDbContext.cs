using AdService.Domain.Aggregates;
using AdService.Domain.Entities;

namespace AdService.Application.Data;

public interface IAppDbContext
{
    DbSet<Ad> Ads { get; }

    DbSet<Comment> Comments { get; }

    DbSet<CarOption> CarOptions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}