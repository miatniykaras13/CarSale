using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.FuelTypes.CreateFuelType;

public record CreateFuelTypeCommand(string Name)
    : ICommand<Result<int, List<Error>>>;

internal class CreateFuelTypeCommandHandler(IFuelTypesRepository fuelTypesRepository)
    : ICommandHandler<CreateFuelTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateFuelTypeCommand command, CancellationToken cancellationToken)
    {
        FuelType fuelType = new()
        {
            Name = command.Name,
        };

        await fuelTypesRepository.AddAsync(fuelType, cancellationToken);

        return Result.Success<int, List<Error>>(fuelType.Id);
    }
}

