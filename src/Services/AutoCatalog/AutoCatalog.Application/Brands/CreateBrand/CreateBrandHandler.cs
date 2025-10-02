using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.CQRS;

namespace AutoCatalog.Application.Brands.CreateBrand;

public record CreateBrandCommand(string Name, string Country, int YearFrom, int YearTo)
    : ICommand<Result<int, List<Error>>>;

internal class CreateBrandCommandHandler(IBrandsRepository brandsRepository)
    : ICommandHandler<CreateBrandCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        Brand brand = new()
        {
            Name = command.Name, Country = command.Country, YearFrom = command.YearFrom, YearTo = command.YearTo,
        };

        await brandsRepository.AddAsync(brand, cancellationToken);

        return Result.Success<int, List<Error>>(brand.Id);
    }
}