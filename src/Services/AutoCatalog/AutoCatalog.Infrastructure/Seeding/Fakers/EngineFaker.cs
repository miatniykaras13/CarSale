using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;
using Bogus;

namespace AutoCatalog.Seeding.Fakers;

public static class EngineFaker
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

    public static Engine[] Generate(int count, Generation[] generations)
    {
        var faker = new Faker<Engine>()
            .UseSeed(7)
            .RuleFor(e => e.Id, _ => 0)
            .RuleFor(e => e.Name, f => f.PickRandom(_engines))
            .RuleFor(e => e.Generation, f => f.PickRandom(generations))
            .RuleFor(e => e.FuelType, f => f.PickRandom<FuelType>())
            .RuleFor(e => e.HorsePower, f => f.Random.Int(100, 1000))
            .RuleFor(e => e.Volume, f => f.Random.Float(1F, 5F))
            .RuleFor(e => e.TorqueNm, f => f.Random.Int(700, 2000));
        return faker.Generate(count).ToArray();
    }
}