using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using CSharpFunctionalExtensions;

namespace AutoCatalog.Infrastructure.Repositories;

public class ModelsRepository : IModelsRepository
{
    public Task<Result<Model, Error>> GetByIdAsync(int id, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> AddAsync(Model model, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> UpdateAsync(Model model, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> DeleteAsync(Model model, CancellationToken cancellationToken) => throw new NotImplementedException();
}