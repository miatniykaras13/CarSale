using FluentValidation;

namespace ProfileService.Application.Commands.UpdateMyProfilePicture;

public class UpdateMyProfilePictureCommandValidator : AbstractValidator<UpdateMyProfilePictureCommand>
{
    public UpdateMyProfilePictureCommandValidator()
    {
        RuleFor(x => x.KeycloakId)
            .NotEmpty().WithMessage("Keycloak ID must be a non-empty GUID.");

        RuleFor(x => x.Stream).NotNull().WithMessage("A picture file is required.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .MaximumLength(255).WithMessage("File name must not exceed 255 characters.");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Content type is required.")
            .Must(value => value.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            .WithMessage("The picture file must have an image content type.");
    }
}