namespace AdService.Contracts.Ads.Default.Snapshots;

public record SellerSnapshotDto(
    Guid SellerId,
    string? DisplayName,
    PhoneNumberDto? PhoneNumber);