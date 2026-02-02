using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<TransmissionType> TransmissionTypes =>
    [
        new()
        {
            Name = "Manual",
        },
        new()
        {
            Name = "Automatic",
        },
        new()
        {
            Name = "CVT",
        },
    ];
}