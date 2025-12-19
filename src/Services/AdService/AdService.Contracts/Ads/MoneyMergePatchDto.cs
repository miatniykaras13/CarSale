namespace AdService.Contracts.Ads;

public record MoneyMergePatchDto(int? Amount, CurrencyMergePatchDto? CurrencyDto);