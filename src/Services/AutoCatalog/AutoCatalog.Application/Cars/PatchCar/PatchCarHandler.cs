using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Application.Cars.PatchCar;

public record PatchCarCommand(
    Guid Id,
    TransmissionType? TransmissionType,
    AutoDriveType? AutoDriveType,
    int? YearFrom,
    int? YearTo,
    Guid? PhotoId,
    float? Consumption,
    float? Acceleration,
    int? FuelTankCapacity,
    DimensionsDto? DimensionsDto) : ICommand<Result<Guid, List<Error>>>;

internal class PatchCarCommandHandler(ICarsRepository carsRepository) : ICommandHandler<PatchCarCommand, Result<Guid, List<Error>>>
{
    public async Task<Result<Guid, List<Error>>> Handle(PatchCarCommand command, CancellationToken cancellationToken)
    {
        TypeAdapterConfig<PatchCarCommand, Car>
            .NewConfig()
            .IgnoreNullValues(true);

        var carResult = await carsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([carResult.Error]);
        var car = carResult.Value;

        command.Adapt(car);

        await carsRepository.UpdateAsync(car, cancellationToken);

        return Result.Success<Guid, List<Error>>(car.Id);
    }
}