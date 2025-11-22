using AutoCatalog.Domain.Specs;
using Bogus;

namespace AutoCatalog.Infrastructure.Seeding.Fakers;

public static class BrandFaker
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

    public static Brand[] Generate(int count)
    {
        var faker = new Faker<Brand>()
            .UseSeed(7)
            .RuleFor(b => b.Id, _ => 0)
            .RuleFor(b => b.Name, f => _brands[f.IndexFaker % _brands.Length])
            .RuleFor(b => b.Country, f => f.Address.Country())
            .RuleFor(b => b.YearFrom, f => f.Random.Int(1901, 1950))
            .RuleFor(b => b.YearTo, f => f.Random.Int(1952, DateTime.UtcNow.Year));
        return faker.Generate(count).ToArray();
    }
}