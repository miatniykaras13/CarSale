using FluentValidation;

namespace ProfileService.Application.Commands.DeleteProfileById;

public class DeleteProfileByIdCommandValidator : AbstractValidator<DeleteProfileByIdCommand>
{
    public DeleteProfileByIdCommandValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty().WithMessage("Profile ID must be a non-empty GUID.");
    }
}
