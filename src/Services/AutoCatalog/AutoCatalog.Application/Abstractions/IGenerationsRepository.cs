using AutoCatalog.Application.Generations;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace AutoCatalog.Application.Abstractions;

public interface IGenerationsRepository : IRepository<int, Generation, GenerationFilter>
{
    Task<Result<List<Generation>, Error>> GetByModelIdAsync(GenerationFilter filter, SortParameters sortParameters, PageParameters pageParameters, int modelId, CancellationToken cancellationToken);
}