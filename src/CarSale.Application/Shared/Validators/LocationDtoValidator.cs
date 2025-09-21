using CarSale.Contracts.Shared;
using FluentValidation;

namespace CarSale.Application.Shared.Validators;

public class LocationDtoValidator : AbstractValidator<LocationDto>
{
    public LocationDtoValidator()
    {
        RuleFor(x => x.City).NotEmpty().WithMessage("City must not be empty");
        RuleFor(x => x.Region).NotEmpty().WithMessage("Region must not be empty");
    }
}