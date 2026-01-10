namespace AdService.Contracts.Ads.MergePatch;

public record LocationDto()
{
    public string? Region { get; init; }

    public string? City { get; init; }
}