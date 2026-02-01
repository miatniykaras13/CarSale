namespace AutoCatalog.Application.Generations;

public record GenerationFilter(
    int? ModelId,
    string? ModelName,
    int? YearFrom,
    int? YearTo);