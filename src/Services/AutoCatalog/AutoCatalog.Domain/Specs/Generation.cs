namespace AutoCatalog.Domain.Specs;

public class Generation
{
    public int Id { get; init; }

    public required int ModelId { get; init; }

    public Model Model { get; init; } = null!;

    public required string Name { get; init; }

    public required int YearFrom { get; init; }

    public bool IsActive => YearTo is null;

    public int? YearTo { get; set; }

    public List<Engine> Engines { get; init; } = [];
}