using AdService.Application.Commands.UploadImage;
using FluentValidation;

namespace AdService.Application.Queries.GetAdById;

public class GetAdByIdQueryValidator : AbstractValidator<GetAdByIdQuery>
{
    public GetAdByIdQueryValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
    }
}