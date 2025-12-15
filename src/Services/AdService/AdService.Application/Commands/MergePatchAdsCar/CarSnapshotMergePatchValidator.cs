using AdService.Contracts.Cars;
using AdService.Domain.ValueObjects;
using FluentValidation;

namespace AdService.Application.Commands.MergePatchAdsCar;

public class CarSnapshotMergePatchValidator : AbstractValidator<CarSnapshotMergePatchDto>
{
    public CarSnapshotMergePatchValidator()
    {
        When(x => x.ModelId.HasValue, () =>
        {
            RuleFor(x => x.BrandId)
                .NotNull()
                .WithMessage("BrandId is required when ModelId is provided.");
        });


        When(x => x.GenerationId.HasValue, () =>
        {
            RuleFor(x => x.ModelId)
                .NotNull()
                .WithMessage("ModelId is required when GenerationId is provided.");
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
    }
}