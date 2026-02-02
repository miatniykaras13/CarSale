namespace AutoCatalog.Application.Cars.Dtos;

public record EngineDto(
    int Id,
    string Name,
    FuelTypeDto FuelType,
    float Volume,
    int HorsePower,
    int TorqueNm);