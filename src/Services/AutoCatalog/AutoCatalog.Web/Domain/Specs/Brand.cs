namespace AutoCatalog.Web.Domain.Specs;

public class Brand
{
    public required int Id { get; set; }

    public required string NameRu { get; set; }

    public required string NameEn { get; set; }

    public required int YearFrom { get; set; }

    public required int YearTo { get; set; }
}