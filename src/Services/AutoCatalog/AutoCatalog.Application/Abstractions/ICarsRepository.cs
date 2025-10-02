using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Application.Abstractions;

public interface ICarsRepository : IRepository<Guid, Car>
{
    Task<Result<List<Car>, Error>> GetByBrandIdAsync(int brandId, CancellationToken ct);

    Task<Result<List<Car>, Error>> GetByModelIdAsync(int modelId, CancellationToken ct);

    Task<Result<List<Car>, Error>> GetByGenerationIdAsync(int generationId, CancellationToken ct);

    Task<Result<List<Car>, Error>> GetByEngineIdAsync(int engineId, CancellationToken ct);
}