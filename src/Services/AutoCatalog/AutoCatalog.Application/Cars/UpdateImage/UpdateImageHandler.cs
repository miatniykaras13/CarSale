using AutoCatalog.Application.Abstractions.FileStorage;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Options;
using Microsoft.Extensions.Options;

namespace AutoCatalog.Application.Cars.UpdateImage;

public record UpdateImageCommand(
    Guid CarId,
    Stream Stream,
    string FileName,
    string ContentType) : ICommand<Result<Guid, List<Error>>>;

public class UpdateImageHandler(
    IFileStorage fileStorage,
    ICarsRepository carsRepository,
    IOptions<FileStorageOptions> fileStorageOptions) : ICommandHandler<UpdateImageCommand, Result<Guid, List<Error>>>
{
    public async Task<Result<Guid, List<Error>>> Handle(UpdateImageCommand command, CancellationToken ct)
    {
        var carResult = await carsRepository.GetByIdAsync(command.CarId, ct);
        if (carResult.IsFailure) return Result.Failure<Guid, List<Error>>(carResult.Error);

        var maxFileSize = fileStorageOptions.Value.MaxFileSize;
        if (command.Stream.Length > maxFileSize)
        {
            return Result.Failure<Guid, List<Error>>(Error.Validation(
                "file.size",
                $"File size must be less than {maxFileSize}"));
        }

        var car = carResult.Value;

        var imageId = await fileStorage.UploadLargeFileAsync(command.Stream, command.FileName, command.ContentType, ct);

        car.PhotoId = imageId;

        await carsRepository.UpdateAsync(car, ct);

        return Result.Success<Guid, List<Error>>(imageId);
    }
}