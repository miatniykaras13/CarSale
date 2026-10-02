using FluentValidation;

namespace ProfileService.Application.Queries.GetMyProfile;

public class GetMyProfileQueryValidator : AbstractValidator<GetMyProfileQuery>
{
    public GetMyProfileQueryValidator()
    {
        RuleFor(x => x.KeycloakId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Keycloak ID is required.")
            .MaximumLength(255).WithMessage("Keycloak ID must not exceed 255 characters.");
    }
}
