using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class BodyTypeSnapshotFaker
{
    private static readonly string[] _bodyTypes = ["Sedan", "SUV", "Crossover", "Hatchback", "Coupe"];

    public static BodyTypeSnapshot[] Fake(int amount)
    {
        var faker = new Faker<BodyTypeSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var bodyType = _bodyTypes[f.IndexFaker % _bodyTypes.Length];
                var result = BodyTypeSnapshot.Of(f.Random.Int(1), bodyType);

                if (result.IsFailure)
                    throw new InvalidOperationException($"BodyTypeSnapshot faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}