using AdService.Contracts.Ads;

namespace AdService.Application.Commands.MergePatchAdsPrice;

public record MergePatchAdsPriceCommand(Guid AdId, MoneyMergePatchDto PriceDto) : ICommand<UnitResult<Error>>;