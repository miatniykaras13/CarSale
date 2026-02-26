namespace AdService.Application.Queries;

public record AdFilter(
    int? BrandId,
    string? BrandName,
    int? ModelId,
    string? ModelName,
    int? GenerationId,
    string? GenerationName,
    int? DriveTypeId,
    string? DriveTypeName,
    int? BodyTypeId,
    string? BodyTypeName,
    int? FuelTypeId,
    string? FuelTypeName,
    int? TransmissionTypeId,
    string? TransmissionTypeName,
    int? PriceFrom,
    int? PriceTo,
    int? MileageFrom,
    int? MileageTo,
    int? YearFrom,
    int? YearTo);