using AutoCatalog.Application.Models;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Abstractions;

public interface IModelsRepository : IRepository<int, Model, ModelFilter>
{
    Task<Result<List<Model>, Error>> GetByBrandIdAsync(ModelFilter filter, SortParameters sortParameters, PageParameters pageParameters, int brandId, CancellationToken ct);
}