using AutoCatalog.Domain.Enums;

namespace AutoCatalog.Application.Engines;

public record EngineFilter(
    FuelType? FuelType,
    float? Volume,
    int? HorsePower,
    int? TorqueNm
    );
