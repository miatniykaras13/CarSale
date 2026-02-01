namespace AutoCatalog.Application.Cars.Dtos;

public record CarDto(
    Guid Id,
    BrandDto Brand,
    ModelDto ModelId,
    GenerationDto GenerationId,
    EngineDto EngineId,
    TransmissionTypeDto TransmissionType,
    AutoDriveTypeDto AutoDriveType,
    string? PhotoUrl,
    float Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto Dimensions);