using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.FuelTypes.UpdateFuelType;

public record UpdateFuelTypeCommand(int Id, string Name)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateFuelTypeCommandHandler(IFuelTypesRepository fuelTypesRepository)
    : ICommandHandler<UpdateFuelTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(UpdateFuelTypeCommand command, CancellationToken cancellationToken)
    {
        var fuelTypeResult = await fuelTypesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (fuelTypeResult.IsFailure)
            return Result.Failure<int, List<Error>>([fuelTypeResult.Error]);

        var fuelType = fuelTypeResult.Value;

        command.Adapt(fuelType);

        await fuelTypesRepository.UpdateAsync(fuelType, cancellationToken);

        return Result.Success<int, List<Error>>(fuelType.Id);
    }
}