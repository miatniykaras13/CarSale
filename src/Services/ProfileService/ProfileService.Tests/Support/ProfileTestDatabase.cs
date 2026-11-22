using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Aggregates;
using ProfileService.Infrastructure.Postgres.Data;

namespace ProfileService.Tests.Support;

internal sealed class ProfileTestDatabase : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _options;

    public ProfileTestDatabase()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"ProfileServiceTests-{Guid.NewGuid()}")
            .Options;
    }

    public TestAppDbContext CreateContext() => new(_options);

    public async Task SeedAsync(params UserProfile[] profiles)
    {
        await using var context = CreateContext();
        context.UserProfiles.AddRange(profiles);
        await context.SaveChangesAsync();
    }

    public async Task<UserProfile?> FindProfileAsync(Guid profileId)
    {
        await using var context = CreateContext();
        return await context.UserProfiles.AsNoTracking().SingleOrDefaultAsync(x => x.Id == profileId);
    }

    public void Dispose()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();
    }
}

internal sealed class TestAppDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options)
{
    public int SaveChangesCalls { get; private set; }

    public DbUpdateException? SaveFailure { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCalls++;
        return SaveFailure is { } exception
            ? Task.FromException<int>(exception)
            : base.SaveChangesAsync(cancellationToken);
    }
}
