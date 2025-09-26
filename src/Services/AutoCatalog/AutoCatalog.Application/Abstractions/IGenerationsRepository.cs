using AutoCatalog.Domain.Specs;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace AutoCatalog.Application.Abstractions;

public interface IGenerationsRepository
{
    Task<Result<Generation, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Result<int, Error>> AddAsync(Generation generation, CancellationToken cancellationToken);

    Task<Result<int, Error>> UpdateAsync(Generation generation, CancellationToken cancellationToken);

    Task<Result<int, Error>> DeleteAsync(Generation generation, CancellationToken cancellationToken);
}