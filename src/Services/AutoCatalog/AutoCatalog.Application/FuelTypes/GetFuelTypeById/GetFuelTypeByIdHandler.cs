using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.FuelTypes.GetFuelTypeById;

public record GetFuelTypeByIdQuery(int Id) : IQuery<Result<FuelType, List<Error>>>;

public class GetFuelTypeByIdQueryHandler(IFuelTypesRepository fuelTypesRepository)
    : IQueryHandler<GetFuelTypeByIdQuery, Result<FuelType, List<Error>>>
{
    public async Task<Result<FuelType, List<Error>>> Handle(
        GetFuelTypeByIdQuery query,
        CancellationToken cancellationToken)
    {
        var fuelTypeResult = await fuelTypesRepository.GetByIdAsync(query.Id, cancellationToken);
        if (fuelTypeResult.IsFailure)
            return Result.Failure<FuelType, List<Error>>([fuelTypeResult.Error]);

        var fuelType = fuelTypeResult.Value;
        return Result.Success<FuelType, List<Error>>(fuelType);
    }
}