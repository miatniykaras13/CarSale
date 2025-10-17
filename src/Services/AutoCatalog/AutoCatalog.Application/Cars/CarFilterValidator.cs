namespace AutoCatalog.Application.Cars;

public class CarFilterValidator : AbstractValidator<CarFilter>
{
    public CarFilterValidator()
    {
        RuleFor(x => x.Acceleration)
            .GreaterThan(0).WithMessage("Acceleration must be greater than 0");

        RuleFor(x => x.Consumption)
            .GreaterThan(0).WithMessage("Consumption must be greater than 0");

        RuleFor(x => x.FuelTankCapacity)
            .GreaterThan(0).WithMessage("Fuel tank capacity must be greater than 0");

        RuleForEach(x => x.AutoDriveType)
            .IsInEnum().WithMessage("Auto drive type is invalid");

        RuleForEach(x => x.TransmissionType)
            .IsInEnum().WithMessage("Transmission type is invalid");

        RuleFor(x => x.Year)
            .GreaterThan(1900).LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Year is invalid");
    }
}