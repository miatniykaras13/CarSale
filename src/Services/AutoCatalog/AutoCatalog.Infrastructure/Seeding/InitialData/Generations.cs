using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<Generation> Generations =>
    [
        new()
        {
            Name = "IV",
            ModelId = 1,
            YearFrom = 1998,
            YearTo = 2005,
        },
    ];
}