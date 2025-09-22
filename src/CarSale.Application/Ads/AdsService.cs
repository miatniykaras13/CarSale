using CarSale.Application.Ads.Factories;
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
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CarSale.Application.Ads;

public class AdsService(
    IAdsRepository adsRepository,
    ILogger<AdsService> logger,
    IValidator<CreateAdDto> createAdDtoValidator) : IAdsService
{
    public async Task<Result<Guid>> Create(
        CreateAdDto adDto,
        CancellationToken cancellationToken)
    {
        var validationResult = await createAdDtoValidator.ValidateAsync(adDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            string errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<Guid>(errors);
        }


        var adResult = AdFactory.FromDto(adDto);
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