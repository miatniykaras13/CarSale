using AutoCatalog.Application.FuelTypes.CreateFuelType;
using AutoCatalog.Application.TransmissionTypes.CreateTransmissionType;

namespace AutoCatalog.Application.BodyTypes.CreateBodyType;

public class CreateTransmissionTypeCommandValidator : AbstractValidator<CreateTransmissionTypeCommand>
{
    public CreateTransmissionTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
    }
}