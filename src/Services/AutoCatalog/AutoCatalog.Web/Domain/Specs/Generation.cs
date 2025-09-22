namespace AutoCatalog.Web.Domain.Specs;

public class Generation
{
    public required int Id { get; set; }

    public required Model Model { get; set; }

    public required string NameRu { get; set; }

    public required string NameEn { get; set; }
}