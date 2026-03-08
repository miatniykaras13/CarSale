using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Application.Options;
using AdService.Contracts.Files;
using AdService.Domain.ValueObjects;
using Microsoft.Extensions.Options;

namespace AdService.Application.Commands.UploadImage;

public class UploadImageHandler(
    IFileStorage fileStorage,
    IAppDbContext dbContext,
    IOptions<FileStorageOptions> fileStorageOptions,
    IOptions<ImageDefaultOptions> options) : ICommandHandler<UploadImageCommand, Result<Guid, List<Error>>>
{
    private readonly ImageDefaultOptions _imageDefaultOptions = options.Value;

    public async Task<Result<Guid, List<Error>>> Handle(UploadImageCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == command.AdId, ct);

        if (ad is null)
            return Result.Failure<Guid, List<Error>>(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        if (ad.Seller.SellerId != command.UserId)
        {
            return Result.Failure<Guid, List<Error>>(Error.Forbidden(
                "ad",
                $"Authenticated user does not own the ad"));
        }

        var maxFileSize = fileStorageOptions.Value.MaxFileSize;

        if (!command.ContentType.Contains("image"))
        {
            return Result.Failure<Guid, List<Error>>(Error.Validation(
                "file_content_type",
                $"File's content type {command.ContentType} is not supported. File must be an image."));
        }

        if (command.Stream.Length > maxFileSize)
        {
            return Result.Failure<Guid, List<Error>>(Error.Validation(
                "file_size",
                $"File's size must be less than {maxFileSize}"));
        }

        var imageId = await fileStorage.UploadFileAsync(command.Stream, command.FileName, command.ContentType, ct);

        var imageResult = AdImage.Of(imageId);
        if (imageResult.IsFailure)
            return Result.Failure<Guid, List<Error>>(imageResult.Error);

        var adImageResult = ad.AddImages([imageResult.Value]);

        if (adImageResult.IsFailure) return Result.Failure<Guid, List<Error>>(adImageResult.Error);

        await GenerateThumbnails(imageResult.Value, ct);

        await dbContext.SaveChangesAsync(ct);

        return Result.Success<Guid, List<Error>>(imageId);
    }

    private async Task<UnitResult<Error>> GenerateThumbnails(AdImage image, CancellationToken ct)
    {
        var smallThumbnailDto = new ThumbnailDto(
            _imageDefaultOptions.SmallThumbnailWidth,
            _imageDefaultOptions.SmallThumbnailHeight);

        var mediumThumbnailDto = new ThumbnailDto(
            _imageDefaultOptions.MediumThumbnailWidth,
            _imageDefaultOptions.MediumThumbnailHeight);

        var largeThumbnailDto = new ThumbnailDto(
            _imageDefaultOptions.LargeThumbnailWidth,
            _imageDefaultOptions.LargeThumbnailHeight);

        var smallThumbnailId =
            await fileStorage.GenerateThumbnailAsync(image.Id, smallThumbnailDto, ct);

        var mediumThumbnailId =
            await fileStorage.GenerateThumbnailAsync(image.Id, mediumThumbnailDto, ct);

        var largeThumbnailId =
            await fileStorage.GenerateThumbnailAsync(image.Id, largeThumbnailDto, ct);

        var smallThumbnailResult = Thumbnail.Of(
            smallThumbnailId,
            image.Id,
            smallThumbnailDto.Width,
            smallThumbnailDto.Height);

        var mediumThumbnailResult = Thumbnail.Of(
            mediumThumbnailId,
            image.Id,
            mediumThumbnailDto.Width,
            mediumThumbnailDto.Height);

        var largeThumbnailResult = Thumbnail.Of(
            largeThumbnailId,
            image.Id,
            largeThumbnailDto.Width,
            largeThumbnailDto.Height);

        image.AddThumbnail(smallThumbnailResult.Value);
        image.AddThumbnail(mediumThumbnailResult.Value);
        image.AddThumbnail(largeThumbnailResult.Value);

        return UnitResult.Success<Error>();
    }
}