using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<Brand> Brands =>
    [
        new()
        {
            Country = "Germany",
            Name = "Volkswagen",
            YearFrom = 1937,
        },
        new()
        {
            Country = "Germany",
            Name = "Audi",
            YearFrom = 1909,
        },
        new()
        {
            Country = "Germany",
            Name = "BMW",
            YearFrom = 1926,
        },
        new()
        {
            Country = "Germany",
            Name = "Mercedes-Benz",
            YearFrom = 1886,
        },
        new()
        {
            Country = "Germany",
            Name = "Messerschmitt",
            YearFrom = 1953,
            YearTo = 1964,
        },
        new()
        {
            Country = "Germany",
            Name = "Porsche",
            YearFrom = 1929,
        },
        new()
        {
            Country = "Japan",
            Name = "Toyota",
            YearFrom = 1935,
        },
        new()
        {
            Country = "Japan",
            Name = "Nissan",
            YearFrom = 1933,
        },
        new()
        {
            Country = "Japan",
            Name = "Honda",
            YearFrom = 1937,
        },
        new()
        {
            Country = "Italy",
            Name = "Alfa Romeo",
            YearFrom = 1910,
        },
    ];
}