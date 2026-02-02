using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Infrastructure.Seeding;

public static class SeedingExtensions
{
    public static DbContextOptionsBuilder SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) =>
            {
                if (context.Set<Brand>().Any())
                    return;
                
                context.Set<TransmissionType>().AddRange(InitialData.InitialData.TransmissionTypes);
                context.SaveChanges();
                
                context.Set<BodyType>().AddRange(InitialData.InitialData.BodyTypes);
                context.SaveChanges();
                
                context.Set<FuelType>().AddRange(InitialData.InitialData.FuelTypes);
                context.SaveChanges();
                
                context.Set<AutoDriveType>().AddRange(InitialData.InitialData.DriveTypes);
                context.SaveChanges();
                
                context.Set<Brand>().AddRange(InitialData.InitialData.Brands);
                context.SaveChanges();
                
                context.Set<Model>().AddRange(InitialData.InitialData.Models);
                context.SaveChanges();
                
                context.Set<Generation>().AddRange(InitialData.InitialData.Generations);
                context.SaveChanges();
                
                context.Set<Engine>().AddRange(InitialData.InitialData.Engines);
                context.SaveChanges();
                
                context.Set<Car>().AddRange(InitialData.InitialData.Cars);
                context.SaveChanges();
            })
            .UseAsyncSeeding(async (context, _, ct) =>
            {
                if (await context.Set<Brand>().AnyAsync(ct))
                    return;

                await context.Set<TransmissionType>().AddRangeAsync(InitialData.InitialData.TransmissionTypes, ct);
                await context.SaveChangesAsync(ct);
                
                await context.Set<BodyType>().AddRangeAsync(InitialData.InitialData.BodyTypes, ct);
                await context.SaveChangesAsync(ct);
                
                await context.Set<FuelType>().AddRangeAsync(InitialData.InitialData.FuelTypes, ct);
                await context.SaveChangesAsync(ct);
                
                await context.Set<AutoDriveType>().AddRangeAsync(InitialData.InitialData.DriveTypes, ct);
                await context.SaveChangesAsync(ct);
                
                await context.Set<Brand>().AddRangeAsync(InitialData.InitialData.Brands, ct);
                await context.SaveChangesAsync(ct);
                
                await context.Set<Model>().AddRangeAsync(InitialData.InitialData.Models, ct);
                await context.SaveChangesAsync(ct);
                
                await context.Set<Generation>().AddRangeAsync(InitialData.InitialData.Generations, ct);
                await context.SaveChangesAsync(ct);
                
                await context.Set<Engine>().AddRangeAsync(InitialData.InitialData.Engines, ct);
                await context.SaveChangesAsync(ct);
                
                await context.Set<Car>().AddRangeAsync(InitialData.InitialData.Cars, ct);
                await context.SaveChangesAsync(ct);
            });
        return optionsBuilder;
    }
}