namespace AutoCatalog.Domain.Specs;

public class Model
{
    public int Id { get; init; }

    public required int BrandId { get; init; }

    public Brand Brand { get; init; } = null!;

    public required string Name { get; init; }

    public List<Generation> Generations { get; init; } = [];
}