using AdService.Contracts.Ads;

namespace AdService.Application.Commands.MergePatchAdsLocation;

public record MergePatchAdsLocationCommand(Guid AdId, LocationMergePatchDto LocationDto) : ICommand<UnitResult<Error>>;