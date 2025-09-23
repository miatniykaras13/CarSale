namespace AutoCatalog.Web.Domain.Specs;

public class Generation
{
    public required int Id { get; set; }


    public required int ModelId { get; set; }

    public Model Model { get; set; } = null!;


    public required string Name { get; set; }

    public List<Engine> Engines { get; set; } = [];
}