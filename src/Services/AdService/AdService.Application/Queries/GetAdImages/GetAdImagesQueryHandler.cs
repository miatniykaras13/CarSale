using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Contracts.Ads.Default;
using AdService.Domain.Enums;

namespace AdService.Application.Queries.GetAdImages;

public class GetAdImagesQueryHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage)
    : IQueryHandler<GetAdImagesQuery, Result<IEnumerable<AdImageDto>, List<Error>>>
{
    public async Task<Result<IEnumerable<AdImageDto>, List<Error>>> Handle(
        GetAdImagesQuery query,
        CancellationToken ct)
    {
        var isUserAuthorized = query.UserId is not null;

        var ad = await dbContext.Ads
            .AsNoTracking()
            .Include(a => a.Images)
                .ThenInclude(i => i.Thumbnails)
            .FirstOrDefaultAsync(a => a.Id == query.AdId, ct);

        if (ad is null ||
            (ad.Status is not (AdStatus.PUBLISHED or AdStatus.ARCHIVED or AdStatus.SOLD) &&
             (!isUserAuthorized || ad.Seller.SellerId != query.UserId!.Value)))
        {
            return Result.Failure<IEnumerable<AdImageDto>, List<Error>>(Error.NotFound(
                "ad",
                $"Ad with id {query.AdId} not found"));
        }

        var imageDtos = new List<AdImageDto>();

        foreach (var image in ad.Images)
        {
            var imageUrl = await fileStorage.GetDownloadLinkAsync(image.Id, 600, ct);

            var thumbnailDtos = new List<AdThumbnailDto>();
            foreach (var thumbnail in image.Thumbnails)
            {
                var thumbnailUrl = await fileStorage.GetDownloadLinkAsync(thumbnail.Id, 600, ct);
                thumbnailDtos.Add(new AdThumbnailDto(thumbnail.Id, thumbnailUrl, thumbnail.Width, thumbnail.Height));
            }

            imageDtos.Add(new AdImageDto(image.Id, imageUrl, thumbnailDtos));
        }

        return Result.Success<IEnumerable<AdImageDto>, List<Error>>(imageDtos);
    }
}

