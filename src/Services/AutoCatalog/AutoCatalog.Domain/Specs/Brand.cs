namespace AutoCatalog.Domain.Specs;

public class Brand
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required string Country { get; init; }

    public required int YearFrom { get; init; }

    public required int YearTo { get; init; }

    public List<Model> Models { get; init; } = [];
}