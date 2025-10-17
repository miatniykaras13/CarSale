using AutoCatalog.Domain.Enums;

namespace AutoCatalog.Application.Cars;

public record CarFilter(
    string? BrandName,
    string? ModelName,
    string? GenerationName,
    string? EngineName,
    TransmissionType[]? TransmissionType,
    AutoDriveType[]? AutoDriveType,
    int? Year,
    decimal? Consumption,
    decimal? Acceleration,
    int? FuelTankCapacity);