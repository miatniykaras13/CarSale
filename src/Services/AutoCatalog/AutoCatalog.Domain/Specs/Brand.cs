namespace AutoCatalog.Domain.Specs;

public class Brand
{
    public int Id { get; set; }

    public required string Name { get; init; }

    public required string Country { get; init; }

    public required int YearFrom { get; init; }

    public bool IsActive => YearTo is null;

    public int? YearTo { get; set; }

    public List<Model> Models { get; init; } = [];
}