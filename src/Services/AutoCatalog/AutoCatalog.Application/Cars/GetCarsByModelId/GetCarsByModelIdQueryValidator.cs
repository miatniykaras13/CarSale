namespace AutoCatalog.Application.Cars.GetCarsByModelId;

public class GetCarsByModelIdQueryValidator : AbstractValidator<GetCarsByModelIdQuery>
{
    public GetCarsByModelIdQueryValidator()
    {
        RuleFor(x => x.ModelId).NotEmpty().WithMessage("Model Id is required");

        RuleFor(x => x.Filter).SetValidator(new CarFilterValidator());
    }
}