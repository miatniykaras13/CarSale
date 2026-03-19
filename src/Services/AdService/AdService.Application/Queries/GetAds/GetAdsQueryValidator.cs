using AdService.Application.Queries.GetAdById;
using FluentValidation;

namespace AdService.Application.Queries.GetAds;

public class GetAdsQueryValidator : AbstractValidator<GetAdByIdQuery>
{
    public GetAdsQueryValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
    }
}