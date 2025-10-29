using AdService.Contracts.Shared;

namespace AdService.Contracts.Ads;

public record CreateAdDto(
    string Title,
    string Description,
    long Views,
    MoneyDto MoneyDto,
    LocationDto LocationDto,
    Guid UserId,
    CarVoDto CarVoDto,
    CarConfigurationDto CarConfigurationDto);