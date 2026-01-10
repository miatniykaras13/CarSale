namespace AdService.Contracts.Ads.MergePatch;

public record MoneyDto()
{
    public int? Amount { get; set; }
    public CurrencyDto? Currency { get; set; }
}