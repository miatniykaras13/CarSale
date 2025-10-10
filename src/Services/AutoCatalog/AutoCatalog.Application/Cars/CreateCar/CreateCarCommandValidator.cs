using AutoCatalog.Application.Cars.CreateCar;

namespace AutoCatalog.Application.Cars.CreateCar;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("Brand Id is required");

        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("Model Id is required");

        RuleFor(x => x.GenerationId)
            .NotEmpty().WithMessage("Generation Id is required");

        RuleFor(x => x.EngineId)
            .NotEmpty().WithMessage("Engine Id is required");

        RuleFor(x => x.PhotoId)
            .NotEmpty().WithMessage("Photo Id is required");

        RuleFor(x => x.Acceleration)
            .NotEmpty().WithMessage("Acceleration is required")
            .GreaterThan(0).WithMessage("Acceleration must be greater than 0");

        RuleFor(x => x.Consumption)
            .NotEmpty().WithMessage("Consumption is required")
            .GreaterThan(0).WithMessage("Consumption must be greater than 0");

        RuleFor(x => x.DimensionsDto.Length)
            .NotNull().NotEmpty().WithMessage("Dimensions.Length is required")
            .GreaterThan(0).WithMessage("Dimensions.Length must be greater than 0");

        RuleFor(x => x.DimensionsDto.Width)
            .NotNull().NotEmpty().WithMessage("Dimensions.Width is required")
            .GreaterThan(0).WithMessage("Dimensions.Width must be greater than 0");

        RuleFor(x => x.DimensionsDto.Height)
            .NotNull().NotEmpty().WithMessage("Dimensions.Height is required")
            .GreaterThan(0).WithMessage("Dimensions.Height must be greater than 0");

        RuleFor(x => x.FuelTankCapacity)
            .NotEmpty().WithMessage("Fuel tank capacity is required")
            .GreaterThan(0).WithMessage("Fuel tank capacity must be greater than 0");

        RuleFor(x => x.AutoDriveType)
            .NotEmpty().WithMessage("Auto drive type is required")
            .IsInEnum().WithMessage("Auto drive type is invalid");

        RuleFor(x => x.TransmissionType)
            .NotEmpty().WithMessage("Transmission type is required")
            .IsInEnum().WithMessage("Transmission type is invalid");
    }
}