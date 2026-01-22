namespace AdService.Contracts.Ads.MergePatch;

public record SellerSnapshotDto
{
    public string? DisplayName { get; set; }

    public PhoneNumberDto? Phone { get; set; }
}