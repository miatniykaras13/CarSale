namespace AutoCatalog.Application.Cars.GetCarsByGenerationId;

public class GetCarsByGenerationIdQueryValidator : AbstractValidator<GetCarsByGenerationIdQuery>
{
    public GetCarsByGenerationIdQueryValidator()
    {
        RuleFor(x => x.GenerationId).NotEmpty().WithMessage("Generation Id is required");

        RuleFor(x => x.Filter).SetValidator(new CarFilterValidator());
    }
}