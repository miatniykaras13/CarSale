using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class ModelSnapshotFaker
{
    public static ModelSnapshot[] Fake(int amount, BrandSnapshot[] brands)
    {
        var faker = new Faker<ModelSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var model = f.Vehicle.Model();
                var brandId = brands[f.IndexFaker % brands.Length].Id;
                var result = ModelSnapshot.Of(f.Random.Int(1), model, brandId);

                if (result.IsFailure)
                    throw new InvalidOperationException($"ModelSnapshot faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}