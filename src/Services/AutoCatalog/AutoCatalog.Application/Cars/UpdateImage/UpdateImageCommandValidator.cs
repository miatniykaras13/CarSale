namespace AutoCatalog.Application.Cars.UpdateImage;

public class UpdateImageCommandValidator : AbstractValidator<UpdateImageCommand>
{
    public UpdateImageCommandValidator()
    {
        RuleFor(x => x.CarId).NotEmpty().WithMessage("Car's id is required");
        RuleFor(x => x.FileName)
            .NotEmpty().NotNull().WithMessage("File name is required")
            .MaximumLength(255).WithMessage("File name cannot exceed 255 characters");
        RuleFor(x => x.ContentType)
            .Must(c => c.StartsWith("image")).WithMessage("The content type must be an image");
    }
}