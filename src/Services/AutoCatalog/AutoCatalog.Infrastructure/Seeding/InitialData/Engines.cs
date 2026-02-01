using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<Engine> Engines =>
    [
        new()
        {
            Name = "1.9 TDI AT",
            FuelTypeId = 1,
            GenerationId = 1,
            HorsePower = 90,
            TorqueNm = 210,
            Volume = 1.9f,
        },
    ];
}