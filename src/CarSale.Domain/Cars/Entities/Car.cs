using CSharpFunctionalExtensions;

namespace CarSale.Domain.Cars.Entities;

public class Car : Entity<Guid>
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string Brand { get; set; }

    public required string Model { get; set; }

    public string? Year { get; set; }

    public required string Generation { get; set; }

    public decimal AveragePriceInUsd { get; set; }

    public int TotalSold { get; set; }
}