using AdService.Contracts.Ads.Default;

namespace AdService.Contracts.Ads.MergePatch;

public record AdNoCarDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public MoneyDto? Price { get; set; }

    public LocationDto? Location { get; set; }

    public SellerSnapshotDto? Seller { get; set; }
}