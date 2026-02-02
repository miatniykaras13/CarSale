using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.FuelTypes.GetFuelTypes;

public record GetFuelTypesQuery(
    FuelTypeFilter Filter,
    SortParameters SortParameters,
    PageParameters PageParameters) : IQuery<Result<List<FuelType>, List<Error>>>;

public class GetFuelTypesQueryHandler(
    IFuelTypesRepository fuelTypesRepository) : IQueryHandler<GetFuelTypesQuery, Result<List<FuelType>, List<Error>>>
{
    public async Task<Result<List<FuelType>, List<Error>>> Handle(
        GetFuelTypesQuery query,
        CancellationToken cancellationToken)
    {
        var fuelTypeResult = await fuelTypesRepository.GetAllAsync(
            query.Filter,
            query.SortParameters,
            query.PageParameters,
            cancellationToken);

        if (fuelTypeResult.IsFailure)
            return Result.Failure<List<FuelType>, List<Error>>([fuelTypeResult.Error]);

        var fuelTypes = fuelTypeResult.Value;
        return Result.Success<List<FuelType>, List<Error>>(fuelTypes);
    }
}