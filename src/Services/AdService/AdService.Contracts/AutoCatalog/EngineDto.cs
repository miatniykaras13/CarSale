namespace AdService.Contracts.AutoCatalog;

public record EngineDto(
    int Id,
    int GenerationId,
    string Name,
    FuelTypeDto FuelType,
    float Volume,
    int HorsePower,
    int TorqueNm);