using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Repositories;

public class EnginesRepository : IEnginesRepository
{
    public Task<Result<Engine, Error>> GetByIdAsync(int id, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> AddAsync(Engine engine, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> UpdateAsync(Engine engine, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> DeleteAsync(Engine engine, CancellationToken cancellationToken) => throw new NotImplementedException();
}