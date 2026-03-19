using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Brands.CreateBrand;

public record CreateBrandCommand(string Name, string Country, int YearFrom, int? YearTo)
    : ICommand<Result<int, List<Error>>>;

internal class CreateBrandCommandHandler(IBrandsRepository brandsRepository)
    : ICommandHandler<CreateBrandCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.GetByName(command.Name, cancellationToken);

        if (brandResult.IsSuccess)
        {
            return Result.Failure<int, List<Error>>(Error.Conflict(
                "brand.name",
                "Cannot create two brands with the same names."));
        }

        if (brandResult.IsFailure && brandResult.Error.Type is not ErrorType.NOT_FOUND)
            return Result.Failure<int, List<Error>>(brandResult.Error);

        Brand brand = new()
        {
            Name = command.Name, Country = command.Country, YearFrom = command.YearFrom, YearTo = command.YearTo,
        };

        await brandsRepository.AddAsync(brand, cancellationToken);

        return Result.Success<int, List<Error>>(brand.Id);
    }
}