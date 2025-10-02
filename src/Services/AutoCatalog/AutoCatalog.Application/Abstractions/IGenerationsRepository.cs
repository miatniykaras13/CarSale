using AutoCatalog.Domain.Specs;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace AutoCatalog.Application.Abstractions;

public interface IGenerationsRepository : IRepository<int, Generation>
{
    Task<Result<List<Generation>, Error>> GetByModelIdAsync(int modelId, CancellationToken cancellationToken);
}