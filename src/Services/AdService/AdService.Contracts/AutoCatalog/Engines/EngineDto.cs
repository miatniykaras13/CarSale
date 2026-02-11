using AdService.Contracts.AutoCatalog.FuelTypes;

namespace AdService.Contracts.AutoCatalog.Engines;

public record EngineDto(
    int Id,
    int GenerationId,
    string Name,
    FuelTypeDto FuelType,
    float Volume,
    int HorsePower,
    int TorqueNm);