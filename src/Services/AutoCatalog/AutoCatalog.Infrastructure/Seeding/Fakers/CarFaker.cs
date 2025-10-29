using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using Bogus;

namespace AutoCatalog.Infrastructure.Seeding.Fakers;

public static class CarFaker
{
    public static Car[] Generate(
        int count,
        Brand[] brands,
        Model[] models,
        Generation[] generations,
        Engine[] engines)
    {
        var dimensionsFaker = new Faker<Dimensions>()
            .UseSeed(7)
            .RuleFor(d => d.Height, f => f.Random.Int(1000, 2000))
            .RuleFor(d => d.Width, f => f.Random.Int(1000, 2000))
            .RuleFor(d => d.Length, f => f.Random.Int(2000, 5000));

        var faker = new Faker<Car>()
            .UseSeed(7)
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Brand, f => f.PickRandom(brands))
            .RuleFor(c => c.Model, f => f.PickRandom(models))
            .RuleFor(c => c.Generation, f => f.PickRandom(generations))
            .RuleFor(c => c.Engine, f => f.PickRandom(engines))
            .RuleFor(c => c.Acceleration, f => f.Random.Decimal(3, 15))
            .RuleFor(c => c.Consumption, f => f.Random.Decimal(4.5m, 20))
            .RuleFor(c => c.FuelTankCapacity, f => f.Random.Int(30, 100))
            .RuleFor(c => c.YearFrom, (f, c) => f.Random.Int(c.Brand.YearFrom, c.Brand.YearTo))
            .RuleFor(c => c.YearTo, (f, c) => f.Random.Int(c.YearFrom, c.Brand.YearTo))
            .RuleFor(c => c.AutoDriveType, f => f.PickRandom<AutoDriveType>())
            .RuleFor(c => c.TransmissionType, f => f.PickRandom<TransmissionType>())
            .RuleFor(c => c.PhotoId, f => f.Random.Guid())
            .RuleFor(c => c.Dimensions, _ => dimensionsFaker.Generate());
        return faker.Generate(count).ToArray();
    }
}