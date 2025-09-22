using AutoCatalog.Web.Domain.Specs;

namespace AutoCatalog.Web.Domain.Cars;

public class Сar
{
    public required int Id { get; set; }

    public required Brand Brand { get; set; }

    public required Model Model { get; set; }

    public required Generation Generation { get; set; }

    public required int Engine { get; set; }

    public required Transmission Transmission { get; set; }

    public required int YearFrom { get; set; }

    public required int YearTo { get; set; }

    public required Guid PhotoId { get; set; }

    public required int Consumption { get; set; }

    public required float Acceleration { get; set; }

    public required int FuelTankCapacity { get; set; }

    public required Dimensions Dimensions { get; set; }
}