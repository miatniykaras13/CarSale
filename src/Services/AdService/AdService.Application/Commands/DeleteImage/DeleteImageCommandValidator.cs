using AdService.Application.Commands.CreateAd;
using FluentValidation;

namespace AdService.Application.Commands.DeleteImage;

public class DeleteImageCommandValidator : AbstractValidator<DeleteImageCommand>
{
    public DeleteImageCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
        RuleFor(x => x.ImageId).NotEmpty().WithMessage("Image's id is required");
    }
}