using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using CSharpFunctionalExtensions;

namespace AutoCatalog.Infrastructure.Repositories;

public class ModelsRepository(AppDbContext context) : IModelsRepository
{
    public async Task<Result<Model, Error>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var model = await context.Models.FindAsync(id, cancellationToken);
        if (model == null)
        {
            return Result.Failure<Model, Error>(Error.NotFound(
                nameof(Model).ToLower(),
                $"Model with id {id} not found."));
        }

        return Result.Success<Model, Error>(model);
    }

    public async Task<Result<int, Error>> AddAsync(Model model, CancellationToken cancellationToken)
    {
        await context.Models.AddAsync(model, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(model.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(Model model, CancellationToken cancellationToken)
    {
        context.Models.Update(model);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(model.Id);
    }

    public async Task<Result<int, Error>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var modelResult = await GetByIdAsync(id, cancellationToken);
        if (modelResult.IsFailure)
        {
            return Result.Failure<int, Error>(modelResult.Error);
        }

        var model = modelResult.Value;
        context.Models.Remove(model);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(id);
    }
}