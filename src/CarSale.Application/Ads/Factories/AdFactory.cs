using CarSale.Application.Shared.Factories;
using CarSale.Contracts.Ads;
using CarSale.Domain.Ads.Aggregates;
using CarSale.Domain.Ads.Enums;
using CarSale.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSale.Application.Ads.Factories
{
    public class AdFactory
    {
        public static Result<Ad> FromDto(CreateAdDto adDto)
        {
            var locationDto = adDto.LocationDto;

            var moneyResult = MoneyFactory.FromDto(adDto.MoneyDto);
            var carVoResult = CarVoFactory.FromDto(adDto.CarVoDto);
            var carConfigurationResult = CarConfigurationFactory.FromDto(adDto.CarConfigurationDto);

            var locationResult = Location.Of(locationDto.Region, locationDto.City);

            var validationVoResult = Result.Combine(moneyResult, carVoResult, carConfigurationResult, locationResult);
            if (validationVoResult.IsFailure)
                return Result.Failure<Ad>(validationVoResult.Error);

            var adResult = Ad.Create(
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
                return Result.Failure<Ad>(adResult.Error);

            return Result.Success(adResult.Value);
        }
    }
}
