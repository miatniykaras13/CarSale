using CarSale.Application.Ads.Factories;
using CarSale.Application.Ads.Interfaces;
using CarSale.Contracts.Ads;
using CarSale.Domain.Ads.Aggregates;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
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
        ValidationResult? validationResult = await createAdDtoValidator.ValidateAsync(adDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            string errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<Guid>(errors);
        }


        Result<Ad> adResult = AdFactory.FromDto(adDto);
        if (adResult.IsFailure)
        {
            return Result.Failure<Guid>(adResult.Error);
        }

        await adsRepository.AddAsync(adResult.Value, cancellationToken);

        return Result.Success(adResult.Value.Id);
    }


    public async Task Delete(
        Guid id,
        CancellationToken cancellationToken) =>
        await adsRepository.DeleteAsync(id, cancellationToken);

    public async Task Get(
        Guid id,
        CancellationToken cancellationToken)
    {
    }
}