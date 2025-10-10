using AutoCatalog.Application.Engines;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Abstractions;

public interface IEnginesRepository : IRepository<int, Engine, EngineFilter>
{
    Task<Result<List<Engine>, Error>> GetByGenerationIdAsync(EngineFilter filter, SortParameters sortParameters, PageParameters pageParameters, int generationId, CancellationToken cancellationToken);
}