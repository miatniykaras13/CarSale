namespace AdService.Contracts.Ads.Default;

public record AdImageDto(Guid ImageId, string Url, IEnumerable<AdThumbnailDto> Thumbnails);

public record AdThumbnailDto(Guid ThumbnailId, string Url, int Width, int Height);


