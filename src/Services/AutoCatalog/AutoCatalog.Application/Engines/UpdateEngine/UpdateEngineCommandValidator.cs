namespace AutoCatalog.Application.Engines.UpdateEngine;

public class UpdateEngineCommandValidator : AbstractValidator<UpdateEngineCommand>
{
    public UpdateEngineCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be greater than 0");

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