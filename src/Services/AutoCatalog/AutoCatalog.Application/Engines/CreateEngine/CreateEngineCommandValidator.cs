using AutoCatalog.Application.Engines.CreateEngine;

namespace AutoCatalog.Application.Engines.CreateEngine;

public class CreateEngineCommandValidator : AbstractValidator<CreateEngineCommand>
{
    public CreateEngineCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.GenerationId)
            .NotEmpty().WithMessage("Generation Id is required");

        RuleFor(x => x.HorsePower)
            .NotEmpty().WithMessage("Horse power is required");

        RuleFor(x => x.TorqueNm)
            .NotEmpty().WithMessage("Torque is required");

        RuleFor(x => x.Volume)
            .NotEmpty().WithMessage("Volume is required");

        RuleFor(x => x.FuelTypeId)
            .NotEmpty().WithMessage("Fuel type id is required");
    }
}