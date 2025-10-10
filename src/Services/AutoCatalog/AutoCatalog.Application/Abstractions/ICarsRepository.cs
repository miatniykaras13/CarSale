using AutoCatalog.Application.Cars;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Abstractions;

public interface ICarsRepository : IRepository<Guid, Car, CarFilter>
{
    Task<Result<List<Car>, Error>> GetByBrandIdAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, int brandId, CancellationToken ct);

    Task<Result<List<Car>, Error>> GetByModelIdAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, int modelId, CancellationToken ct);

    Task<Result<List<Car>, Error>> GetByGenerationIdAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, int generationId, CancellationToken ct);

    Task<Result<List<Car>, Error>> GetByEngineIdAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, int engineId, CancellationToken ct);
}