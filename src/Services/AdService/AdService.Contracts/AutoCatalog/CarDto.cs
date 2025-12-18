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
    int YearFrom,
    int YearTo,
    Guid PhotoId,
    decimal Consumption,
    decimal Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto);