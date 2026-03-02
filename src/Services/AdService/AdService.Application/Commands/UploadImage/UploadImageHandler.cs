using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Application.Options;
using AdService.Domain.ValueObjects;
using Microsoft.Extensions.Options;

namespace AdService.Application.Commands.UploadImage;

public class UploadImageHandler(
    IFileStorage fileStorage,
    IAppDbContext dbContext,
    IOptions<FileStorageOptions> fileStorageOptions) : ICommandHandler<UploadImageCommand, Result<Guid, List<Error>>>
{
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

        await dbContext.SaveChangesAsync(ct);

        return Result.Success<Guid, List<Error>>(imageId);
    }
}