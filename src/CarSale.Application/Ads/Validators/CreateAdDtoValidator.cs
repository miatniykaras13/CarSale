using CarSale.Application.Shared.Validators;
using CarSale.Contracts.Ads;
using CarSale.Domain.Ads.Aggregates;
using FluentValidation;


namespace CarSale.Application.Ads.Validators;

public class CreateAdDtoValidator : AbstractValidator<CreateAdDto>
{
    public CreateAdDtoValidator()
    {
        int maxTitleLength = Ad.MaxTitleLength;
        int minTitleLength = Ad.MinTitleLength;
        int maxDescriptionLength = Ad.MaxDescriptionLength;

        RuleFor(x => x.Title).NotEmpty().MinimumLength(minTitleLength).MaximumLength(maxTitleLength)
            .WithMessage($"Title's length must be between {minTitleLength} and {maxDescriptionLength}.");

        RuleFor(x => x.Description).MaximumLength(maxDescriptionLength)
            .WithMessage($"Description's length must be between 0 and {maxDescriptionLength}.");

        RuleFor(x => x.Views).NotEmpty().LessThan(long.MaxValue).WithMessage("Too much views.");

        RuleFor(x => x.MoneyDto).SetValidator(new MoneyDtoValidator());

        RuleFor(x => x.LocationDto).SetValidator(new LocationDtoValidator());

        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.CarVoDto).SetValidator(new CarVoDtoValidator());

        RuleFor(x => x.CarConfigurationDto).SetValidator(new CarConfigurationDtoValidator());
    }
}