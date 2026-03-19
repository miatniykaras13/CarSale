using AdService.Application.Abstractions.Data;

namespace AdService.Application.Commands.DeleteCarOption;

public class DeleteCarOptionHandler(IAppDbContext dbContext)
    : ICommandHandler<DeleteCarOptionCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(DeleteCarOptionCommand command, CancellationToken ct)
    {
        var carOption = await dbContext.CarOptions.FindAsync([command.CarOptionId], ct).AsTask();

        if (carOption is null)
        {
            return UnitResult.Failure<List<Error>>(Error.NotFound(
                "car_option",
                $"Car option with id {command.CarOptionId} not found"));
        }

        var isUsedByAnyAd = await dbContext.Ads
            .AnyAsync(a => a.CarOptions.Any(co => co.Id == command.CarOptionId), ct);

        if (isUsedByAnyAd)
        {
            return UnitResult.Failure<List<Error>>(Error.Conflict(
                "car_option.in_use",
                $"Car option with id {command.CarOptionId} is used by one or more ads and cannot be deleted"));
        }

        dbContext.CarOptions.Remove(carOption);
        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<List<Error>>();
    }
}

