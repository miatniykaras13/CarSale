using System.Reflection;
using AdService.Application.Abstractions.Data;
using AdService.Domain.Aggregates;
using AdService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdService.Infrastructure.Postgres.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Ad> Ads { get; set; }

    public DbSet<CarOption> CarOptions { get; set; }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}