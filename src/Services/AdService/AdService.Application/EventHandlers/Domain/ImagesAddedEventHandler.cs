using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Application.Options;
using AdService.Contracts.Files;
using AdService.Domain.Events;
using AdService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdService.Application.EventHandlers.Domain;

public class ImagesAddedEventHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage,
    IOptions<ImageDefaultOptions> options,
    ILogger<ImagesAddedEventHandler> logger) : INotificationHandler<ImagesAddedEvent>
{
    private readonly ImageDefaultOptions _imageDefaultOptions = options.Value;

    public async Task Handle(ImagesAddedEvent notification, CancellationToken cancellationToken)
    {
        var images = notification.Images;

        var imageTasks = images.Select(async image =>
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
                await fileStorage.GenerateThumbnailAsync(image.Id, smallThumbnailDto, cancellationToken);

            var mediumThumbnailId =
                await fileStorage.GenerateThumbnailAsync(image.Id, mediumThumbnailDto, cancellationToken);

            var largeThumbnailId =
                await fileStorage.GenerateThumbnailAsync(image.Id, largeThumbnailDto, cancellationToken);

            var smallThumbnailResult = Thumbnail.Of(
                smallThumbnailId,
                image.Id,
                smallThumbnailDto.Width,
                smallThumbnailDto.Height);

            if (smallThumbnailResult.IsFailure)
            {
                logger.LogError(
                    "Failed to generate small thumbnail. Reason: {error}",
                    smallThumbnailResult.Error.Message);
            }

            var mediumThumbnailResult = Thumbnail.Of(
                mediumThumbnailId,
                image.Id,
                mediumThumbnailDto.Width,
                mediumThumbnailDto.Height);

            if (mediumThumbnailResult.IsFailure)
            {
                logger.LogError(
                    "Failed to generate medium thumbnail. Reason: {error}",
                    mediumThumbnailResult.Error.Message);
            }

            var largeThumbnailResult = Thumbnail.Of(
                largeThumbnailId,
                image.Id,
                largeThumbnailDto.Width,
                largeThumbnailDto.Height);

            if (largeThumbnailResult.IsFailure)
            {
                logger.LogError(
                    "Failed to generate large thumbnail. Reason: {error}",
                    largeThumbnailResult.Error.Message);
            }

            image.AddThumbnail(smallThumbnailResult.Value);
            image.AddThumbnail(mediumThumbnailResult.Value);
            image.AddThumbnail(largeThumbnailResult.Value);
        });

        await Task.WhenAll(imageTasks);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}