using AdService.Application.Abstractions.FileStorage;
using AdService.Domain.Events;
using Microsoft.Extensions.Logging;

namespace AdService.Application.EventHandlers.Domain;

public class ImageRemovedEventHandler(
    IFileStorage fileStorage,
    ILogger<ImageRemovedEventHandler> logger) : INotificationHandler<ImageRemovedEvent>
{
    public async Task Handle(ImageRemovedEvent notification, CancellationToken cancellationToken)
    {
        var isDeleteSucceeded = await fileStorage.DeleteFileAsync(notification.ImageId, cancellationToken);

        if (!isDeleteSucceeded)
            logger.LogError("Failed to delete image with id {ImageId}", notification.ImageId);
    }
}