using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface IModelsRepository
{
    Task<Result<Model, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Result<int, Error>> AddAsync(Model model, CancellationToken cancellationToken);

    Task<Result<int, Error>> UpdateAsync(Model model, CancellationToken cancellationToken);

    Task<Result<int, Error>> DeleteAsync(Model model, CancellationToken cancellationToken);
}