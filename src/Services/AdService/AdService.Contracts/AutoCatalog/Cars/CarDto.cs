namespace AdService.Contracts.AutoCatalog.Cars;

public record CarDto(
    Guid Id,
    BrandDto Brand,
    ModelDto Model,
    GenerationDto Generation,
    EngineDto Engine,
    TransmissionTypeDto TransmissionType,
    AutoDriveTypeDto DriveType,
    BodyTypeDto BodyType,
    string? PhotoUrl,
    float Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto Dimensions);