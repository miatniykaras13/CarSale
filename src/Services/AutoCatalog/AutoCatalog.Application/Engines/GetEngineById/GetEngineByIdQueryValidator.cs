namespace AutoCatalog.Application.Engines.GetEngineById;

public class GetEngineByIdQueryValidator : AbstractValidator<GetEngineByIdQuery>
{
    public GetEngineByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}