using AutoCatalog.Domain.Specs;
using Bogus;

namespace AutoCatalog.Infrastructure.Seeding.Fakers;

public static class BrandFaker
{
    public static Brand[] Generate(int count)
    {
        var faker = new Faker<Brand>()
            .UseSeed(7)
            .RuleFor(b => b.Id, _ => 0)
            .RuleFor(b => b.Name, f => f.Vehicle.Manufacturer())
            .RuleFor(b => b.Country, f => f.Address.Country())
            .RuleFor(b => b.YearFrom, f => f.Random.Int(1901, 1950))
            .RuleFor(b => b.YearTo, f => f.Random.Int(1952, DateTime.Now.Year));
        return faker.Generate(count).ToArray();
    }
}