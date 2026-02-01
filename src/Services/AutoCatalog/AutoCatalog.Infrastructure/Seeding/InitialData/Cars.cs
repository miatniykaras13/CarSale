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
            PhotoId = null, // todo сделать в file management сидирование чтобы был такой же id
            Consumption = 6.3f,
            Dimensions = new Dimensions() { Length = 4376, Height = 1446, Width = 1735 },
        },
    ];
}