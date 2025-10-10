namespace AutoCatalog.Application.Brands;

public record BrandFilter(
    string? Country,
    int? YearFrom,
    int? YearTo);