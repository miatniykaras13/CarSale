using AutoCatalog.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AutoCatalog.Application.Engines;

public record EngineFilter(
    FuelType[]? FuelType,
    float? Volume,
    int? HorsePower,
    int? TorqueNm
    );
