namespace AutoCatalog.Application.Cars.DeleteImage;

public class DeleteImageCommandValidator : AbstractValidator<DeleteImageCommand>
{
    public DeleteImageCommandValidator()
    {
        RuleFor(x => x.CarId).NotEmpty().WithMessage("Car's id is required");
    }
}