using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface IEnginesRepository : IRepository<int, Engine>
{
    Task<Result<List<Engine>, Error>> GetByGenerationIdAsync(int generationId, CancellationToken cancellationToken);
}