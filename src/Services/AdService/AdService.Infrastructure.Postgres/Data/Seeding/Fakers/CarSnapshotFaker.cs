using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class CarSnapshotFaker
{
    private static readonly string[] _colors = ["black", "white", "red", "green", "blue", "pink", "purple", "yellow"];


    public static CarSnapshot[] Fake(
        int amount,
        BrandSnapshot[] brands,
        ModelSnapshot[] models,
        GenerationSnapshot[] generations,
        EngineSnapshot[] engines,
        TransmissionTypeSnapshot[] transmissionTypes,
        AutoDriveTypeSnapshot[] driveTypes,
        BodyTypeSnapshot[] bodyTypes)
    {
        var faker = new Faker<CarSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var engine = engines[f.IndexFaker % engines.Length] with { };
                var generation = generations.First(g => g.Id == engine.GenerationId) with { };
                var model = models.First(m => m.Id == generation.ModelId) with { };
                var brand = brands.First(b => b.Id == model.BrandId) with { };
                var year = f.Random.Int(generation.YearFrom, generation.YearTo); // >1900
                var vin = f.Random.String2(CarSnapshot.REQUIRED_VIN_LENGTH, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
                var mileage = f.Random.Int(0, 500000);
                var consumption = f.Random.Decimal(5, 50);
                var color = f.Random.ArrayElement(_colors);
                var driveType = f.Random.ArrayElement(driveTypes) with { };
                var transmissionType = f.Random.ArrayElement(transmissionTypes) with { };
                var bodyType = f.Random.ArrayElement(bodyTypes) with { };

                var result = CarSnapshot.Of(
                    f.Random.Guid(),
                    brand,
                    model,
                    generation,
                    engine,
                    driveType,
                    transmissionType,
                    bodyType,
                    year,
                    vin,
                    mileage,
                    consumption,
                    color);

                if (result.IsFailure)
                    throw new InvalidOperationException($"CarSnapshot faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}