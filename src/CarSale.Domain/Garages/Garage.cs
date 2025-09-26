namespace CarSale.Domain.Garages;

public class Garage
{
    public Guid Id { get; set; }

    public List<Guid> CarIds { get; set; } = [];

    public required Guid UserId { get; set; }
}