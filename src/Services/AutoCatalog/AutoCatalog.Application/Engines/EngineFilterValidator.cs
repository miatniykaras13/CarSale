using AutoCatalog.Domain.Enums;

namespace AutoCatalog.Application.Engines;

public class EngineFilterValidator : AbstractValidator<EngineFilter>
{
    public EngineFilterValidator()
    {
        RuleFor(x => x.HorsePower)
            .GreaterThan(0).WithMessage("Horse power must be greater than zero");

        RuleForEach(x => x.FuelType)
            .IsInEnum().WithMessage("Fuel type is invalid");

        RuleFor(x => x.TorqueNm)
            .GreaterThan(0).WithMessage("Torque must be greater than zero");

        RuleFor(x => x.Volume)
            .GreaterThan(0).WithMessage("Volume must be greater than zero");
    }
}