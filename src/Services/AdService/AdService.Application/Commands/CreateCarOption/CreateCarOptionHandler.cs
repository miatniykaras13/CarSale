using AdService.Application.Abstractions.Data;
using AdService.Domain.Entities;

namespace AdService.Application.Commands.CreateCarOption;

public class CreateCarOptionHandler(IAppDbContext dbContext)
    : ICommandHandler<CreateCarOptionCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateCarOptionCommand command, CancellationToken ct)
    {
        var exists = await dbContext.CarOptions
            .AnyAsync(co => co.TechnicalName == command.TechnicalName, ct);

        if (exists)
        {
            return Result.Failure<int, List<Error>>(Error.Conflict(
                "car_option",
                $"Car option with technical name {command.TechnicalName} already exists"));
        }

        var carOptionResult = CarOption.Create(command.OptionType, command.Name, command.TechnicalName);

        if (carOptionResult.IsFailure)
            return Result.Failure<int, List<Error>>(carOptionResult.Error);

        await dbContext.CarOptions.AddAsync(carOptionResult.Value, ct);
        await dbContext.SaveChangesAsync(ct);

        return Result.Success<int, List<Error>>(carOptionResult.Value.Id);
    }
}