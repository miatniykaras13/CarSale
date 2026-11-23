using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Domain.Aggregates;

namespace ProfileService.Infrastructure.Postgres.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
