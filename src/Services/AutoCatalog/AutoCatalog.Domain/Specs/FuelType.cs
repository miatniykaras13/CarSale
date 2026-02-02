namespace AutoCatalog.Domain.Specs;

public class FuelType
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public List<Engine> Engines { get; init; } = [];
}