using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class CarSnapshotFaker
{
    private static readonly string[] _colors = ["black", "white", "red", "green", "blue", "pink", "purple", "yellow"];

    private static readonly string[] _generations =
    [
        "Mk1", "Mk2", "Mk3", "Mk4", "Mk5", "Mk6", "Mk7", "Mk8", "Gen1", "Gen2", "Gen3", "Gen4", "Gen5", "Gen6",
        "E30", "E36", "E46", "E90", "F30", "G20", "W123", "W124", "W210", "W211", "W212", "W213", "B5", "B6", "B7",
        "B8", "B9", "XV30", "XV40", "XV50", "XV70"
    ];

    private static readonly string[] _driveTypes = ["FWD", "RWD", "AWD"];

    private static readonly string[] _transmissionTypes = ["Automatic", "Manual"];

    private static readonly string[] _fuelTypes = ["Diesel", "Petrol", "Electro"];

    private static readonly string[] _bodyTypes = ["Sedan", "SUV", "Crossover", "Hatchback", "Coupe"];


    public static CarSnapshot[] Fake(int amount)
    {
        var faker = new Faker<CarSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var brand = f.Vehicle.Manufacturer();
                var model = f.Vehicle.Model();
                var year = f.Random.Int(1990, DateTime.UtcNow.Year); // >1900
                var generation = f.Random.ArrayElement(_generations);
                var vin = f.Random.String2(CarSnapshot.REQUIRED_VIN_LENGTH, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
                var mileage = f.Random.Int(0, 500000);
                var consumption = f.Random.Decimal(100, 500);
                var color = f.Random.ArrayElement(_colors);
                var horsePower = f.Random.Int(50, CarSnapshot.MAX_HORSE_POWER);
                var driveType = f.Random.ArrayElement(_driveTypes);
                var transmissionType = f.Random.ArrayElement(_transmissionTypes);
                var fuelType = f.Random.ArrayElement(_fuelTypes);
                var bodyType = f.Random.ArrayElement(_bodyTypes);

                var result = CarSnapshot.Of(
                    f.Random.Guid(),
                    brand,
                    model,
                    year,
                    generation,
                    vin,
                    mileage,
                    color,
                    horsePower,
                    consumption,
                    driveType,
                    transmissionType,
                    fuelType,
                    bodyType);

                if (result.IsFailure)
                    throw new InvalidOperationException($"CarSnapshot faker failed: {result.Error}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}