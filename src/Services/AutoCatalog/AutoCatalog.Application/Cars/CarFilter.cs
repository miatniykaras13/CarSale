namespace AutoCatalog.Application.Cars;

public record CarFilter(
    int? BrandId,
    string? BrandName,
    int? ModelId,
    string? ModelName,
    int? GenerationId,
    string? GenerationName,
    int? EngineId,
    string? EngineName,
    int? TransmissionTypeId,
    string? TransmissionType,
    int? DriveTypeId,
    string? DriveType,
    int? BodyTypeId,
    string? BodyType,
    float? Consumption,
    float? Acceleration,
    int? FuelTankCapacity);