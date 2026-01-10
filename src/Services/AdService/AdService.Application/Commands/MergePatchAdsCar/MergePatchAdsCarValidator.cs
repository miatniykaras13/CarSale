using AdService.Contracts.Ads;
using AdService.Contracts.Ads.MergePatch;
using AdService.Domain.ValueObjects;
using FluentValidation;

namespace AdService.Application.Commands.MergePatchAdsCar;

public class MergePatchAdsCarValidator : AbstractValidator<CarSnapshotDto>
{
    /*public MergePatchAdsCarValidator()
    {
        When(x => x.Model is not null, () =>
        {
            RuleFor(x => x.Brand)
                .NotNull()
                .WithMessage("BrandId is required when ModelId is provided.");
        });


        When(x => x.Generation is not null, () =>
        {
            RuleFor(x => x.Model)
                .NotNull()
                .WithMessage("Model is required when GenerationId is provided.");
        });


        When(x => x.EngineId.HasValue, () =>
        {
            RuleFor(x => x.GenerationId)
                .NotNull()
                .WithMessage("GenerationId is required when EngineId is provided.");
        });


        RuleFor(x => x.Year)
            .GreaterThan(1900)
            .When(x => x.Year.HasValue)
            .WithMessage("Year must be greater than 1900.");

        RuleFor(x => x.Vin)
            .Length(CarSnapshot.REQUIRED_VIN_LENGTH)
            .When(x => x.Vin != null)
            .WithMessage($"VIN must be {CarSnapshot.REQUIRED_VIN_LENGTH} characters long.");
    }*/
}