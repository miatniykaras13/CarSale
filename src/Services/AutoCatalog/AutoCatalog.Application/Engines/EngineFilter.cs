namespace AutoCatalog.Application.Engines;

public record EngineFilter(
    int? GenerationId,
    string? GenerationName,
    int? FuelTypeId,
    string? FuelType,
    float? Volume,
    int? HorsePower,
    int? TorqueNm);
