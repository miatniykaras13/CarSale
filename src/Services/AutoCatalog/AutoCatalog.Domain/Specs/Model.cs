namespace AutoCatalog.Domain.Specs;

public class Model
{
    public int Id { get; set; }


    public required int BrandId { get; set; }

    public Brand Brand { get; set; } = null!;


    public required string Name { get; set; }

    public List<Generation> Generations { get; set; } = [];
}