namespace AutoCatalog.Domain.Specs;

public class Brand
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required int YearFrom { get; set; }

    public required int YearTo { get; set; }

    public List<Model> Models { get; set; } = [];
}