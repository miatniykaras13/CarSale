using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Brands.UpdateBrand;

public record UpdateBrandCommand(int Id, string Name, string Country, int YearFrom, int? YearTo)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateBrandCommandHandler(IBrandsRepository brandsRepository)
    : ICommandHandler<UpdateBrandCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(UpdateBrandCommand command, CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (brandResult.IsFailure)
            return Result.Failure<int, List<Error>>([brandResult.Error]);

        var brand = brandResult.Value;

        command.Adapt(brand);

        await brandsRepository.UpdateAsync(brand, cancellationToken);

        return Result.Success<int, List<Error>>(brand.Id);
    }
}