namespace AdService.Contracts.AutoCatalog;

public record CarDto(
    Guid Id,
    int BrandId,
    int ModelId,
    int GenerationId,
    int EngineId,
    int TransmissionTypeId,
    int AutoDriveTypeId,
    int BodyTypeId,
    Guid PhotoId,
    decimal Consumption,
    decimal Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto);