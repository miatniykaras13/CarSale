namespace AutoCatalog.Application.Cars.UpdateCar;

public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
{
    public UpdateCarCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("Brand id is required");

        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("Model id is required");

        RuleFor(x => x.GenerationId)
            .NotEmpty().WithMessage("Generation id is required");

        RuleFor(x => x.EngineId)
            .NotEmpty().WithMessage("Engine id is required");

        RuleFor(x => x.BodyTypeId)
            .NotEmpty().WithMessage("Body type id is required");

        RuleFor(x => x.TransmissionTypeId)
            .NotEmpty().WithMessage("Transmission type id is required");

        RuleFor(x => x.DriveTypeId)
            .NotEmpty().WithMessage("Drive type id is required");

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
    }
}