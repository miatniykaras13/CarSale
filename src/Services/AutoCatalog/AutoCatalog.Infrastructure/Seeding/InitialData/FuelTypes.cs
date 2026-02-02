using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<FuelType> FuelTypes =>
    [
        new()
        {
            Name = "Diesel",
        },
        new()
        {
            Name = "Petrol",
        },
        new()
        {
            Name = "Electro",
        },
        new()
        {
            Name = "Hybrid",
        },
    ];
}