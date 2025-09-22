namespace AutoCatalog.Web.Domain.Specs;

public class Engine
{
    public required int Id { get; set; }

    public required FuelType FuelType { get; set; }

    public required Model Model { get; set; }

    public required string Name { get; set; }

    public required float Volume { get; set; }

    public required int HorsePower { get; set; }

    public required int TorqueNm { get; set; }
}