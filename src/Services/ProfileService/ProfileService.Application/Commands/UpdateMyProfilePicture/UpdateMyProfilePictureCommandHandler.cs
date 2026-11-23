using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Abstractions.FileStorage;
using ProfileService.Application.Options;
using ProfileService.Domain.Aggregates;

namespace ProfileService.Application.Commands.UpdateMyProfilePicture;

public class UpdateMyProfilePictureCommandHandler(
    IAppDbContext dbContext,
    IOptions<FileStorageOptions> options,
    IFileStorage fileStorage,
    ILogger<UpdateMyProfilePictureCommandHandler> logger)
    : ICommandHandler<UpdateMyProfilePictureCommand, Result<Guid, List<Error>>>
{
    public async Task<Result<Guid, List<Error>>> Handle(
        UpdateMyProfilePictureCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .SingleOrDefaultAsync(x => x.KeycloakId == command.KeycloakId, cancellationToken);

        if (profile is null)
            return Result.Failure<Guid, List<Error>>(Error.NotFound("profile", "User profile not found."));

        return await UpdateAsync(
            profile, command.Stream, command.FileName, command.ContentType, cancellationToken);
    }

    private async Task<Result<Guid, List<Error>>> UpdateAsync(
        UserProfile profile,
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        if (stream is null || !stream.CanRead || !stream.CanSeek)
        {
            return Result.Failure<Guid, List<Error>>(
                Error.Validation("file", "The picture file must provide a readable, seekable stream."));
        }

        var maxFileSize = options.Value.MaxFileSize;
        if (stream.Length <= 0 || stream.Length > maxFileSize)
        {
            return Result.Failure<Guid, List<Error>>(
                Error.Validation("file_size", $"Picture size must be between 1 and {maxFileSize} bytes."));
        }

        var oldPictureId = profile.Picture;
        var pictureId = await fileStorage.UploadFileAsync(stream, fileName, contentType, cancellationToken);
        var updateResult = profile.UpdatePicture(pictureId);

        if (updateResult.IsFailure)
        {
            await DeleteFileBestEffortAsync(pictureId);
            return Result.Failure<Guid, List<Error>>(updateResult.Error);
        }

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            await DeleteFileBestEffortAsync(pictureId);
            throw;
        }

        if (oldPictureId is { } previousPictureId && previousPictureId != pictureId)
            await DeleteFileBestEffortAsync(previousPictureId);

        return Result.Success<Guid, List<Error>>(pictureId);
    }

    private async Task DeleteFileBestEffortAsync(Guid pictureId)
    {
        if (pictureId == Guid.Empty)
            return;

        try
        {
            using var cleanupCancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            if (!await fileStorage.DeleteFileAsync(pictureId, cleanupCancellation.Token))
                logger.LogWarning("Could not remove unused profile picture {PictureId}.", pictureId);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not remove unused profile picture {PictureId}.", pictureId);
        }
    }
}