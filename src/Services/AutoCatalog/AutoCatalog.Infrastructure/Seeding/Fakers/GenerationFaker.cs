using AutoCatalog.Domain.Specs;
using Bogus;

namespace AutoCatalog.Seeding.Fakers;

public static class GenerationFaker
{
    private static readonly string[] _generations = [
        "Mk1", "Mk2", "Mk3", "Mk4", "Mk5", "Mk6", "Mk7", "Mk8",
        "Gen1", "Gen2", "Gen3", "Gen4", "Gen5", "Gen6",
        "E30", "E36", "E46", "E90", "F30", "G20",
        "W123", "W124", "W210", "W211", "W212", "W213",
        "B5", "B6", "B7", "B8", "B9",
        "XV30", "XV40", "XV50", "XV70"
    ];

    public static Generation[] Generate(int count, Model[] models)
    {
        var faker = new Faker<Generation>()
            .UseSeed(7)
            .RuleFor(m => m.Id, _ => 0)
            .RuleFor(m => m.Name, f => f.PickRandom(_generations))
            .RuleFor(m => m.Model, f => f.PickRandom(models));
        return faker.Generate(count).ToArray();
    }
}