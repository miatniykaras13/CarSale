using AutoCatalog.Application.Models.DeleteModel;

namespace AutoCatalog.Application.Models.DeleteModel;

public class DeleteModelCommandValidator : AbstractValidator<DeleteModelCommand>
{
    public DeleteModelCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}