using CarSale.Contracts.Cars;

namespace CarSale.Contracts.Ads;

public record GetAdDto(string Title, string Description, GetCarDto carDto, List<Guid> Images, string Region, DateTime UpdatedAt, DateTime CreatedAt, long Views);