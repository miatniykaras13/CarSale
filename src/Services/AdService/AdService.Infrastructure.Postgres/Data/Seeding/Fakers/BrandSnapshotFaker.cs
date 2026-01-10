using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class BrandSnapshotFaker
{
    private static readonly string[] _brands =
    [
        "Toyota",
        "Honda",
        "Nissan",
        "Mitsubishi",
        "Mazda",
        "Subaru",
        "Hyundai",
        "Kia",
        "Ford",
        "Chevrolet",
        "Volkswagen",
        "Audi",
        "BMW",
        "Mercedes-Benz",
        "Porsche",
        "Peugeot",
        "Renault",
        "Fiat",
        "Volvo",
        "Jaguar",
        "Land Rover",
        "Ferrari",
        "Lamborghini",
        "Maserati",
        "Cadillac",
        "Buick",
        "Opel",
        "Skoda",
        "Seat",
        "Alfa Romeo"
    ];

    public static BrandSnapshot[] Fake(int amount)
    {
        var faker = new Faker<BrandSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var brand = _brands[f.IndexFaker % _brands.Length];
                var result = BrandSnapshot.Of(f.Random.Int(1), brand);

                if (result.IsFailure)
                    throw new InvalidOperationException($"BrandSnapshot faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}