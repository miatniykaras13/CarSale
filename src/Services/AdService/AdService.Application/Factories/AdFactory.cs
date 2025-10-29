using AdService.Application.Shared.Factories;
using CSharpFunctionalExtensions;

namespace AdService.Application.Factories;

public class AdFactory
{
    /*public static Result<Ad> FromDto(CreateAdDto adDto)
    {
        LocationDto locationDto = adDto.LocationDto;

        Result<Money> moneyResult = MoneyFactory.FromDto(adDto.MoneyDto);
        Result<CarVo> carVoResult = CarVoFactory.FromDto(adDto.CarVoDto);
        Result<CarConfiguration> carConfigurationResult = CarConfigurationFactory.FromDto(adDto.CarConfigurationDto);

        Result<Location> locationResult = Location.Of(locationDto.Region, locationDto.City);

        Result validationVoResult = Result.Combine(moneyResult, carVoResult, carConfigurationResult, locationResult);
        if (validationVoResult.IsFailure)
        {
            return Result.Failure<Ad>(validationVoResult.Error);
        }

        Result<Ad> adResult = Ad.Create(
            adDto.Title,
            adDto.Description,
            adDto.Views,
            moneyResult.Value,
            locationResult.Value,
            [],
            [],
            [],
            Guid.Parse("569de17a-7751-4767-bf76-083ac158175c"),
            carVoResult.Value,
            carConfigurationResult.Value,
            AdStatus.Unpublished);
        if (adResult.IsFailure)
        {
            return Result.Failure<Ad>(adResult.Error);
        }

        return Result.Success(adResult.Value);
    }*/
}