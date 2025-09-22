namespace AutoCatalog.Web.Domain.Specs;

public class Model
{
    public required int Id { get; set; }

    public required int BrandId { get; set; }

    public required string NameRu { get; set; }

    public required string NameEn { get; set; }
}