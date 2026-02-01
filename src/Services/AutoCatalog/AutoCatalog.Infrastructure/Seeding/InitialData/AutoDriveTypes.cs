using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<AutoDriveType> DriveTypes =>
    [
        new()
        {
            Name = "RWD",
        },
        new()
        {
            Name = "AWD",
        },
        new()
        {
            Name = "FWD",
        },
    ];
}