using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using AutoCatalog.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AutoCatalog.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Car> Cars { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Model> Models { get; set; }

    public DbSet<Generation> Generations { get; set; }

    public DbSet<Engine> Engines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BrandConfiguration());
        modelBuilder.ApplyConfiguration(new ModelConfiguration());
        modelBuilder.ApplyConfiguration(new GenerationConfiguration());
        modelBuilder.ApplyConfiguration(new EngineConfiguration());
        modelBuilder.ApplyConfiguration(new CarConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}