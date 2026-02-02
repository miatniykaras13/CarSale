using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.AutoDriveTypes.GetDriveTypes;

public class GetDriveTypesQueryValidator : AbstractValidator<GetDriveTypesQuery>
{
    public GetDriveTypesQueryValidator()
    {
        RuleFor(x => x.PageParameters).SetValidator(new PageParametersValidator());
        RuleFor(x => x.SortParameters).SetValidator(new SortParametersValidator());
    }
}