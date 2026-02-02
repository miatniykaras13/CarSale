using AutoCatalog.Application.FuelTypes.CreateFuelType;

namespace AutoCatalog.Application.BodyTypes.CreateBodyType;

public class CreateFuelTypeCommandValidator : AbstractValidator<CreateFuelTypeCommand>
{
    public CreateFuelTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
    }
}