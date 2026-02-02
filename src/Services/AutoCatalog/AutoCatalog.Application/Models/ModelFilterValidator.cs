using AutoCatalog.Application.Generations;

namespace AutoCatalog.Application.Models;

public class ModelFilterValidator : AbstractValidator<ModelFilter>
{
    public ModelFilterValidator()
    {
        RuleFor(x => x.BrandId)
            .GreaterThan(0).WithMessage("Brand id must be greater than zero");
    }
}