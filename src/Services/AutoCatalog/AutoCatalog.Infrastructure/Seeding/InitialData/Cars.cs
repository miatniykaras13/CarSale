using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Infrastructure.Seeding.InitialData;

public static partial class InitialData
{
    public static IEnumerable<Car> Cars =>
    [
        new()
        {
            Id = new Guid("4e211d0a-778b-4e00-b1fb-7e5f3fb418ef"),
            BrandId = 1,
            ModelId = 1,
            GenerationId = 1,
            EngineId = 1,
            DriveTypeId = 3,
            TransmissionTypeId = 1,
            BodyTypeId = 1,
            Acceleration = 14.7f,
            FuelTankCapacity = 55,
            PhotoId = new Guid("3b44e9ce-a0ca-4330-b4ee-260ebf5cd07c"),
            Consumption = 6.3f,
            Dimensions = new Dimensions() { Length = 4376, Height = 1446, Width = 1735 },
        },
        new()
        {
            Id = new Guid("6f128cce-f3bd-471c-90fa-30a40423b8e9"),
            BrandId = 1,
            ModelId = 2,
            GenerationId = 2,
            EngineId = 2,
            DriveTypeId = 3,
            TransmissionTypeId = 1,
            BodyTypeId = 1,
            Acceleration = 10.4f,
            FuelTankCapacity = 62,
            PhotoId = new Guid("fc2e73e0-46ab-4384-a093-ba5f00d20fdf"),
            Consumption = 6.3f,
            Dimensions = new Dimensions() { Length = 4703, Height = 1462, Width = 1746 },
        },
        new()
        {
            Id = new Guid("55f6e271-b082-4e9e-8552-4a6cbbe45a7f"),
            BrandId = 7,
            ModelId = 3,
            GenerationId = 3,
            EngineId = 3,
            DriveTypeId = 2,
            TransmissionTypeId = 2,
            BodyTypeId = 5,
            Acceleration = 8.4f,
            FuelTankCapacity = 100,
            PhotoId = new Guid("a61302ad-1968-4892-a820-3aebae57ebbd"),
            Consumption = 16.8f,
            Dimensions = new Dimensions() { Length = 5180, Height = 1880, Width = 1980 },
        },
    ];
}