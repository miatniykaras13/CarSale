namespace CarSale.Contracts;

public record CreateAdDto(string Title, string Description, CreateCarDto carDto, List<Guid> Images, string Region);