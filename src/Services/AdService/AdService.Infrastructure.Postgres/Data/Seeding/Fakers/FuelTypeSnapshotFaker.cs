using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class FuelTypeSnapshotFaker
{
    private static readonly string[] _fuelTypes = ["Diesel", "Petrol", "Electro"];

    public static FuelTypeSnapshot[] Fake(int amount)
    {
        var faker = new Faker<FuelTypeSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var fuelType = _fuelTypes[f.IndexFaker % _fuelTypes.Length];
                var result = FuelTypeSnapshot.Of(f.Random.Int(1), fuelType);

                if (result.IsFailure)
                    throw new InvalidOperationException($"FuelTypeSnapshot faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}