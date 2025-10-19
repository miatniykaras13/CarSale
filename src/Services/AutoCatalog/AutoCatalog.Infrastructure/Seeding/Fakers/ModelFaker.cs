using AutoCatalog.Domain.Specs;
using Bogus;

namespace AutoCatalog.Seeding.Fakers;

public class ModelFaker
{
    public static Model[] Generate(int count, Brand[] brands)
    {
        var faker = new Faker<Model>()
            .UseSeed(7)
            .RuleFor(m => m.Id, _ => 0)
            .RuleFor(m => m.Name, f => f.Vehicle.Model())
            .RuleFor(m => m.Brand, f => f.PickRandom(brands));
        return faker.Generate(count).ToArray();
    }
}