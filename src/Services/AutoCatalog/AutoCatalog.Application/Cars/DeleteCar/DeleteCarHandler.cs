using AutoCatalog.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.DeleteCar;

public record DeleteCarCommand(Guid Id) : ICommand<Result<Unit, List<Error>>>;

public class DeleteCarQueryHandler(ICarsRepository carsRepository)
    : ICommandHandler<DeleteCarCommand, Result<Unit, List<Error>>>
{
    public async Task<Result<Unit, List<Error>>> Handle(DeleteCarCommand command, CancellationToken cancellationToken)
    {
        var carResult = await carsRepository.DeleteAsync(command.Id, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<Unit, List<Error>>([carResult.Error]);

        return Result.Success<Unit, List<Error>>(Unit.Value);
    }
}