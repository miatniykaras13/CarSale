using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface IModelsRepository : IRepository<int, Model>
{
    Task<Result<List<Model>, Error>> GetByBrandIdAsync(int brandId, CancellationToken ct);
}