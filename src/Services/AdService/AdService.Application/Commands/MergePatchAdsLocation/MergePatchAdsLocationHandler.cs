using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.MergePatchAdsLocation;

public class MergePatchAdsLocationCommandHandler(IAppDbContext dbContext)
    : ICommandHandler<MergePatchAdsLocationCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(
        MergePatchAdsLocationCommand command,
        CancellationToken cancellationToken)
    {
        var ad = await dbContext.Ads.FindAsync([command.AdId], cancellationToken);

        if (ad is null)
            return UnitResult.Failure(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        var currentLocation = ad.Location;

        var locationDto = command.LocationDto;

        var newLocationResult = Location.Of(
            region: locationDto.Region ?? currentLocation?.Region,
            city: locationDto.City ?? currentLocation?.City);

        if (newLocationResult.IsFailure) return newLocationResult;

        var updateResult = ad.Update(location: newLocationResult.Value);

        if (updateResult.IsFailure) return updateResult;

        await dbContext.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }
}