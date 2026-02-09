using AutoCatalog.Application.Abstractions.FileStorage;
using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.Cars.DeleteImage;

public record DeleteImageCommand(Guid CarId) : ICommand<UnitResult<List<Error>>>;

public class DeleteImageCommandHandler(
    ICarsRepository carsRepository,
    IFileStorage fileStorage) : ICommandHandler<DeleteImageCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(DeleteImageCommand command, CancellationToken ct)
    {
        var carResult = await carsRepository.GetByIdAsync(command.CarId, ct);

        if (carResult.IsFailure) return UnitResult.Failure<List<Error>>(carResult.Error);

        var car = carResult.Value;

        if (car.PhotoId is null)
        {
            return UnitResult.Failure<List<Error>>(Error.Conflict(
                "car.photo_id",
                "Cannot delete car's image, because image id is null."));
        }

        var success = await fileStorage.DeleteFileAsync(car.PhotoId.Value, ct);

        if (success) car.PhotoId = null;

        await carsRepository.UpdateAsync(carResult.Value, ct);

        return UnitResult.Success<List<Error>>();
    }
}