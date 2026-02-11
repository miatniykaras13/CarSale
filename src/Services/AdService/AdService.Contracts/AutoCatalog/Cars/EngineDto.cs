using AdService.Contracts.AutoCatalog.FuelTypes;

namespace AdService.Contracts.AutoCatalog.Cars;

public record EngineDto(
    int Id,
    string Name,
    FuelTypeDto FuelType,
    float Volume,
    int HorsePower,
    int TorqueNm);