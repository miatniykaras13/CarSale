using FluentValidation;

namespace AdService.Application.Validators;

public class CarConfigurationDtoValidator : AbstractValidator<CarConfigurationDto>
{
    public CarConfigurationDtoValidator()
    {
        RuleFor(x => x.OpticsType).Must(s => Enum.TryParse(s, true, out OpticsType opticsType))
            .WithMessage("There's no such optics type");

        RuleFor(x => x.InteriorType).Must(s => Enum.TryParse(s, true, out InteriorType interiorType))
            .WithMessage("There's no such interior type");

        RuleForEach(x => x.AssistanceOptions).Must(s => Enum.TryParse(s, true, out AssistanceOptions assistanceSystems))
            .WithMessage("There aren't such assistance systems");

        RuleForEach(x => x.ComfortOptions).Must(s => Enum.TryParse(s, true, out ComfortOptions comfortOptions))
            .WithMessage("There aren't such comfort options");

        RuleForEach(x => x.SafetyOptions).Must(s => Enum.TryParse(s, true, out SafetyOptions safetyOptions))
            .WithMessage("There aren't such safety options");
    }
}