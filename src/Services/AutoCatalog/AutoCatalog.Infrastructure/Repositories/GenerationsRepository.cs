using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Repositories;

public class GenerationsRepository : IGenerationsRepository
{
    public Task<Result<Generation, Error>> GetByIdAsync(int id, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> AddAsync(Generation generation, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> UpdateAsync(Generation generation, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> DeleteAsync(Generation generation, CancellationToken cancellationToken) => throw new NotImplementedException();
}