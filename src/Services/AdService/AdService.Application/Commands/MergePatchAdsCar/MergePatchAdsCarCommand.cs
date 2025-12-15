using AdService.Contracts.Cars;

namespace AdService.Application.Commands.MergePatchAdsCar;

public record MergePatchAdsCarCommand(Guid AdId, CarSnapshotMergePatchDto CarDto) : ICommand<UnitResult<Error>>;