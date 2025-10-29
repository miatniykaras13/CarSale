namespace AdService.Contracts.Ads;

public record CarVoDto(
    string Brand,
    string Model,
    int Year,
    string Generation,
    string Vin,
    int Mileage,
    string Color);