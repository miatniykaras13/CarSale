using System.Data;
using System.Reflection;
using AdService.Application.Abstractions.Data;
using AdService.Domain.Aggregates;
using AdService.Domain.Entities;
using AdService.Domain.ValueObjects;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace AdService.Infrastructure.Postgres.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Ad> Ads { get; set; }

    public DbSet<CarOption> CarOptions { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public async Task<T> ReadSnapshotAsync<T>(Func<CancellationToken, Task<T>> read, CancellationToken cancellationToken)
    {
        await using var transaction =
            await Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken);
        var result = await read(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
