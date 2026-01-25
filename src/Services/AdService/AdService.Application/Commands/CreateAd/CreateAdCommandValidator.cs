using FluentValidation;

namespace AdService.Application.Commands.CreateAd;

public class CreateAdCommandValidator : AbstractValidator<CreateAdCommand>
{
    public CreateAdCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
    }
}