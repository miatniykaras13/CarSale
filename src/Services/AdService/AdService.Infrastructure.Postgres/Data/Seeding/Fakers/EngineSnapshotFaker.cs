using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class EngineSnapshotFaker
{
    private static readonly string[] _engines =
    [
        "Orion X2 Turbo",
        "Titan V6 Hybrid",
        "Nova EcoDrive 2.0",
        "Helios TDI 180",
        "Falcon GTX 3.5",
        "Vega FlexFuel 1.8",
        "Atlas TwinCharge 2.2",
        "Zenith Hydra 4.0"
    ];

    public static EngineSnapshot[] Fake(int amount, GenerationSnapshot[] generations, FuelTypeSnapshot[] fuelTypes)
    {
        var faker = new Faker<EngineSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var name = _engines[f.IndexFaker % _engines.Length];
                var fuelType = fuelTypes[f.IndexFaker % fuelTypes.Length] with { };
                var horsePower = f.Random.Int(EngineSnapshot.MIN_HORSE_POWER, EngineSnapshot.MAX_HORSE_POWER);
                var generationId = generations[f.IndexFaker % generations.Length].Id;
                var result = EngineSnapshot.Of(f.Random.Int(1), name, horsePower, fuelType, generationId);

                if (result.IsFailure)
                    throw new InvalidOperationException($"EngineSnapshot faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}