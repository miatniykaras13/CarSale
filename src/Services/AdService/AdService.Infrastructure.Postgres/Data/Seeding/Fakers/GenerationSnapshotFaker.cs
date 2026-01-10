using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class GenerationSnapshotFaker
{
    private static readonly string[] _generations =
    [
        "Mk1", "Mk2", "Mk3", "Mk4", "Mk5", "Mk6", "Mk7", "Mk8",
        "Gen1", "Gen2", "Gen3", "Gen4", "Gen5", "Gen6",
        "E30", "E36", "E46", "E90", "F30", "G20",
        "W123", "W124", "W210", "W211", "W212", "W213",
        "B5", "B6", "B7", "B8", "B9",
        "XV30", "XV40", "XV50", "XV70"
    ];

    public static GenerationSnapshot[] Fake(int amount, ModelSnapshot[] models)
    {
        var faker = new Faker<GenerationSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var generation = _generations[f.IndexFaker % _generations.Length];
                var modelId = models[f.IndexFaker % models.Length].Id;
                var result = GenerationSnapshot.Of(f.Random.Int(1), generation, modelId, f.Random.Int(1900, DateTime.UtcNow.Year - 50), f.Random.Int(DateTime.UtcNow.Year - 49, DateTime.UtcNow.Year));

                if (result.IsFailure)
                    throw new InvalidOperationException($"GenerationSnapshot faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}