using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<Model> Models =>
    [
        new()
        {
            Name = "Jetta",
            BrandId = 1,
        },
        new()
        {
            Name = "Passat",
            BrandId = 1,
        },
    ];
}