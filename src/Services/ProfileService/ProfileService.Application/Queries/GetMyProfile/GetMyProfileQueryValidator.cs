using FluentValidation;

namespace ProfileService.Application.Queries.GetMyProfile;

public class GetMyProfileQueryValidator : AbstractValidator<GetMyProfileQuery>
{
    public GetMyProfileQueryValidator()
    {
        RuleFor(x => x.KeycloakId)
            .NotEmpty().WithMessage("Keycloak ID must be a non-empty GUID.");
    }
}
