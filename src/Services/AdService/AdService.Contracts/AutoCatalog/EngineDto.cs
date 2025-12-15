namespace AdService.Contracts.AutoCatalog;

public record EngineDto(
    int Id,
    int GenerationId,
    string Name,
    int FuelTypeId,
    float Volume,
    int HorsePower,
    int TorqueNm);