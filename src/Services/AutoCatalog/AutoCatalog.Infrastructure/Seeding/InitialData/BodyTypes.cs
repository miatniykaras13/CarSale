using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<BodyType> BodyTypes =>
    [
        new()
        {
            Name = "Sedan",
        },
        new()
        {
            Name = "Pickup",
        },
        new()
        {
            Name = "Hatchback",
        },
        new()
        {
            Name = "Van",
        },
        new()
        {
            Name = "SUV",
        },
        new()
        {
            Name = "Sport",
        },
    ];
}