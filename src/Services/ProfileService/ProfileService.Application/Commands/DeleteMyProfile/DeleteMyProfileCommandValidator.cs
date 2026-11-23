using FluentValidation;

namespace ProfileService.Application.Commands.DeleteMyProfile;

public class DeleteMyProfileCommandValidator : AbstractValidator<DeleteMyProfileCommand>
{
    public DeleteMyProfileCommandValidator()
    {
        RuleFor(x => x.KeycloakId)
            .NotEmpty().WithMessage("Keycloak ID must be a non-empty GUID.");
    }
}
