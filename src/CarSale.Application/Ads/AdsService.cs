using CarSale.Application.Ads.Interfaces;
using CarSale.Application.Ads.Validators;
using CarSale.Contracts;
using CarSale.Contracts.Ads;
using CarSale.Domain.Ads;
using CarSale.Domain.Ads.Aggregates;
using CarSale.Domain.Ads.Enums;
using CarSale.Domain.Ads.ValueObjects;
using CarSale.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace CarSale.Application.Ads;

public class AdsService(IAdsRepository adsRepository, ILogger<AdsService> logger)
{


    public async Task<Result<Guid>> Create(
        CreateAdDto adDto,
        CancellationToken cancellationToken)
    {
        var validator = new CreateAdDtoValidator();
        var validationResult = await validator.ValidateAsync(adDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            string errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<Guid>(errors);
        }

        var currencyDto = adDto.MoneyDto.CurrencyDto;
        var moneyDto = adDto.MoneyDto;
        var carVoDto = adDto.CarVoDto;
        var locationDto = adDto.LocationDto;
        var carConfigurationDto = adDto.CarConfigurationDto;

        var currencyResult = Currency.Of(currencyDto.CurrencyCode);
        if (currencyResult.IsFailure)
            return Result.Failure<Guid>(currencyResult.Error);

        var moneyResult = Money.Of(currencyResult.Value, moneyDto.Amount);
        var carVoResult = CarVo.Of(
            carVoDto.Brand,
            carVoDto.Model,
            carVoDto.Year,
            carVoDto.Generation,
            carVoDto.Vin,
            carVoDto.Mileage,
            carVoDto.Color);
        var carConfigurationResult = CarConfiguration.Of(
            carConfigurationDto.InteriorType,
            carConfigurationDto.SafetyOptions,
            carConfigurationDto.ComfortOptions,
            carConfigurationDto.AssistanceOptions,
            carConfigurationDto.OpticsType);
        var locationResult = Location.Of(locationDto.Region, locationDto.City);

        var validationVoResult = Result.Combine(moneyResult, carVoResult, carConfigurationResult, locationResult);
        if (validationVoResult.IsFailure)
            return Result.Failure<Guid>(validationVoResult.Error);

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
            return Result.Failure<Guid>(adResult.Error);
        
        await adsRepository.AddAsync(adResult.Value, cancellationToken);
        
        return Result.Success(adResult.Value.Id);
    }


    public async Task Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await adsRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task Get(
        Guid id,
        CancellationToken cancellationToken)
    {
        
    }
}