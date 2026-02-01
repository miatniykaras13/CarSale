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
        new()
        {
            Name = "1.9 TDI MT",
            FuelTypeId = 1,
            GenerationId = 2,
            HorsePower = 130,
            TorqueNm = 310,
            Volume = 1.9f,
        },
        new()
        {
            Name = "4.7 AT",
            FuelTypeId = 2,
            GenerationId = 3,
            HorsePower = 240,
            TorqueNm = 427,
            Volume = 4.7f,
        },
    ];
}