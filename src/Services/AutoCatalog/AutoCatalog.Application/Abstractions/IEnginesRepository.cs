using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface IEnginesRepository
{
    Task<Result<Engine, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Result<int, Error>> AddAsync(Engine engine, CancellationToken cancellationToken);

    Task<Result<int, Error>> UpdateAsync(Engine engine, CancellationToken cancellationToken);

    Task<Result<int, Error>> DeleteAsync(int id, CancellationToken cancellationToken);
}