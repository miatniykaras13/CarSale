using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.FuelTypes.DeleteFuelType;

public record DeleteFuelTypeCommand(int Id) : ICommand<UnitResult<List<Error>>>;

public class DeleteFuelTypeQueryHandler(IFuelTypesRepository fuelTypesRepository)
    : ICommandHandler<DeleteFuelTypeCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(DeleteFuelTypeCommand command, CancellationToken cancellationToken)
    {
        var fuelTypeResult = await fuelTypesRepository.DeleteAsync(command.Id, cancellationToken);
        if (fuelTypeResult.IsFailure)
            return UnitResult.Failure<List<Error>>([fuelTypeResult.Error]);

        return UnitResult.Success<List<Error>>();
    }
}