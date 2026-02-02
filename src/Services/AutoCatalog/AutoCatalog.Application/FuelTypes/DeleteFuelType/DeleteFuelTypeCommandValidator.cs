using AutoCatalog.Application.FuelTypes.DeleteFuelType;

namespace AutoCatalog.Application.BodyTypes.DeleteBodyType;

public class DeleteFuelTypeCommandValidator : AbstractValidator<DeleteFuelTypeCommand>
{
    public DeleteFuelTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}