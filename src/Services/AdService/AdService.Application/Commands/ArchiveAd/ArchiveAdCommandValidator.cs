using FluentValidation;

namespace AdService.Application.Commands.ArchiveAd;

public class ArchiveAdCommandValidator : AbstractValidator<ArchiveAdCommand>
{
    public ArchiveAdCommandValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
    }
}