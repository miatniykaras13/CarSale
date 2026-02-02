using System.Reflection;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Car> Cars { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Model> Models { get; set; }

    public DbSet<Generation> Generations { get; set; }

    public DbSet<Engine> Engines { get; set; }

    public DbSet<AutoDriveType> DriveTypes { get; set; }

    public DbSet<BodyType> BodyTypes { get; set; }

    public DbSet<TransmissionType> TransmissionTypes { get; set; }

    public DbSet<FuelType> FuelTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}