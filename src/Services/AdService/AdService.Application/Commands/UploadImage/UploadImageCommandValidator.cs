using AdService.Application.Commands.SubmitAd;
using FluentValidation;

namespace AdService.Application.Commands.UploadImage;

public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
    public UploadImageCommandValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
        RuleFor(x => x.FileName)
            .NotEmpty().NotNull().WithMessage("File name is required")
            .MaximumLength(255).WithMessage("File name cannot exceed 255 characters");
        RuleFor(x => x.ContentType)
            .Must(c => c.StartsWith("image")).WithMessage("The content type must be an image");
    }
}