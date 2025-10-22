namespace AutoCatalog.Domain.Specs;

public class Generation
{
    public int Id { get; init; }

    public required int ModelId { get; init; }

    public Model Model { get; init; } = null!;

    public required string Name { get; init; }

    public List<Engine> Engines { get; init; } = [];
}