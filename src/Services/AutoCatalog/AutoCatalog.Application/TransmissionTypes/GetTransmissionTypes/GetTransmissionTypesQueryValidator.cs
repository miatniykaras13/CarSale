using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.TransmissionTypes.GetTransmissionTypes;

public class GetTransmissionTypesQueryValidator : AbstractValidator<GetTransmissionTypesQuery>
{
    public GetTransmissionTypesQueryValidator()
    {
        RuleFor(x => x.PageParameters).SetValidator(new PageParametersValidator());
        RuleFor(x => x.SortParameters).SetValidator(new SortParametersValidator());
    }
}