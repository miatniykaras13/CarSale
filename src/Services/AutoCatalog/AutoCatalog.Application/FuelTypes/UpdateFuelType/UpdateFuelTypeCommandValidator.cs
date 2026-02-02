using AutoCatalog.Application.AutoDriveTypes.UpdateDriveType;

namespace AutoCatalog.Application.FuelTypes.UpdateFuelType;

public class UpdateFuelTypeCommandValidator : AbstractValidator<UpdateDriveTypeCommand>
{
    public UpdateFuelTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}