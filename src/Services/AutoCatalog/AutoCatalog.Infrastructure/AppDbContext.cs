using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using AutoCatalog.Infrastructure.Configurations;
using AutoCatalog.Seeding.Fakers;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) =>
            {
                if (context.Set<Brand>().Any())
                    return;

                var brandsToSeed = BrandFaker.Generate(20);
                context.Set<Brand>().AddRange(brandsToSeed);
                context.SaveChanges();


                var modelsToSeed = ModelFaker.Generate(20, brandsToSeed);

                context.Set<Model>().AddRange(modelsToSeed);
                context.SaveChanges();

                var generationsToSeed = GenerationFaker.Generate(20, modelsToSeed);

                context.Set<Generation>()
                    .AddRange(generationsToSeed);
                context.SaveChanges();

                var enginesToSeed = EngineFaker.Generate(20, generationsToSeed);

                context.Set<Engine>().AddRange(enginesToSeed);
                context.SaveChanges();

                var carsToSeed = CarFaker.Generate(20, brandsToSeed, modelsToSeed, generationsToSeed, enginesToSeed);

                context.Set<Car>().AddRange(carsToSeed);
                context.SaveChanges();
            })
            .UseAsyncSeeding(async (context, _, ct) =>
            {
                if (await context.Set<Brand>().AnyAsync(ct))
                    return;

                var brandsToSeed = BrandFaker.Generate(20);

                await context.Set<Brand>()
                    .AddRangeAsync(brandsToSeed, ct);
                await context.SaveChangesAsync(ct);


                var modelsToSeed = ModelFaker.Generate(20, brandsToSeed);

                await context.Set<Model>()
                    .AddRangeAsync(modelsToSeed, ct);
                await context.SaveChangesAsync(ct);

                var generationsToSeed = GenerationFaker.Generate(20, modelsToSeed);

                await context.Set<Generation>()
                    .AddRangeAsync(generationsToSeed, ct);
                await context.SaveChangesAsync(ct);

                var enginesToSeed = EngineFaker.Generate(20, generationsToSeed);

                await context.Set<Engine>()
                    .AddRangeAsync(enginesToSeed, ct);
                await context.SaveChangesAsync(ct);

                var carsToSeed = CarFaker.Generate(20, brandsToSeed, modelsToSeed, generationsToSeed, enginesToSeed);

                await context.Set<Car>()
                    .AddRangeAsync(carsToSeed, ct);
                await context.SaveChangesAsync(ct);
            });
    }
}