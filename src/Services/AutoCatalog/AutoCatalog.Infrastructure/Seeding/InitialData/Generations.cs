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
        new()
        {
            Name = "B5 Restyling",
            ModelId = 2,
            YearFrom = 2000,
            YearTo = 2005,
        },
        new()
        {
            Name = "I",
            ModelId = 3,
            YearFrom = 2001,
            YearTo = 2005,
        },
    ];
}