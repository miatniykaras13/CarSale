using FluentValidation;

namespace ProfileService.Application.Queries.GetMyAds;

public class GetMyAdsQueryValidator : AbstractValidator<GetMyAdsQuery>
{
    public GetMyAdsQueryValidator()
    {
        RuleFor(x => x.KeycloakId)
            .NotEmpty().WithMessage("Keycloak ID must be a non-empty GUID.");
    }
}
