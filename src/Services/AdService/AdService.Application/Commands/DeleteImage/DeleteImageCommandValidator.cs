using AdService.Application.Commands.CreateAd;
using FluentValidation;

namespace AdService.Application.Commands.DeleteImage;

public class DeleteImageCommandValidator : AbstractValidator<CreateAdCommand>
{
    public DeleteImageCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
    }
}