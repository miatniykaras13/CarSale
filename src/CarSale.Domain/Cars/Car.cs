namespace CarSale.Domain.Cars;

public class Car
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string Brand { get; set; }

    public required string Model { get; set; }

    public string? Year { get; set; }

    public required string Generation { get; set; }

    public Equipment? Equipment { get; set; }

    public decimal AveragePriceInUsd { get; set; }

    public int TotalSold { get; set; }
}