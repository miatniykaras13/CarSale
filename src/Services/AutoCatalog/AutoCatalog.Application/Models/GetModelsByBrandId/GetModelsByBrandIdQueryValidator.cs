namespace AutoCatalog.Application.Models.GetModelsByBrandId;

public class GetModelsByBrandIdQueryValidator : AbstractValidator<GetModelsByBrandIdQuery>
{
    public GetModelsByBrandIdQueryValidator()
    {
        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("Brand Id is required");
    }
}