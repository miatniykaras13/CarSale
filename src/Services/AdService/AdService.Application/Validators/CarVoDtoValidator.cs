using FluentValidation;

namespace AdService.Application.Validators;

public class CarVoDtoValidator : AbstractValidator<CarVoDto>
{
    public CarVoDtoValidator()
    {
        int requiredVinLength = CarVo.RequiredVinLength;

        RuleFor(x => x.Brand).NotEmpty().WithMessage("Brand should not be empty");

        RuleFor(x => x.Model).NotEmpty().WithMessage("Model should not be empty");

        RuleFor(x => x.Color).NotEmpty().WithMessage("Color should not be empty");

        RuleFor(x => x.Generation).NotEmpty().WithMessage("Generation should not be empty");

        RuleFor(x => x.Mileage).NotEmpty().WithMessage("Mileage should not be empty").GreaterThan(0)
            .WithMessage("Mileage should be greater than 0").LessThanOrEqualTo(1000000)
            .WithMessage("Mileage should be less than 1000000 km");

        RuleFor(x => x.Vin).NotEmpty().WithMessage("Vin should not be empty").Length(requiredVinLength)
            .WithMessage($"Vin length should be equal to {requiredVinLength}");

        RuleFor(x => x.Year).NotEmpty().WithMessage("Year should not be empty").LessThan(DateTime.Now.Year)
            .WithMessage("Year should be less than the current year").GreaterThan(1900)
            .WithMessage("Year should be greater than the 1900");
    }
}