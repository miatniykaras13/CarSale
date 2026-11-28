using FluentValidation;

namespace AdService.Application.Queries.GetAds;

public class GetAdsQueryValidator : AbstractValidator<GetAdsQuery>
{
    public GetAdsQueryValidator()
    {
        RuleFor(x => x.PageParameters.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageParameters.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.PageParameters)
            .Must(page => ((long)page.PageNumber - 1) * page.PageSize <= int.MaxValue)
            .WithMessage("The requested page offset is too large.");
        RuleFor(x => x.Sort)
            .Must(sort => sort is GetAdsQuery.Newest or GetAdsQuery.Oldest)
            .WithMessage("Sort must be 'newest' or 'oldest'.");
    }
}
