namespace AutoCatalog.Application.Models.GetModelById;

public class GetModelByIdQueryValidator : AbstractValidator<GetModelByIdQuery>
{
    public GetModelByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}