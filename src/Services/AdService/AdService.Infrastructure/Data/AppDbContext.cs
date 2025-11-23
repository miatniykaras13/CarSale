using System.Reflection;
using AdService.Domain.Aggregates;
using AdService.Domain.Entities;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;
using AdService.Infrastructure.Data.Interceptors;
using AdService.Infrastructure.Data.Seeding.Fakers;
using Microsoft.EntityFrameworkCore;

namespace AdService.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
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