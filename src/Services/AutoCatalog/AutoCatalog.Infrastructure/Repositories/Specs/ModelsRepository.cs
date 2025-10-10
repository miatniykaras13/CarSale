using System.Collections.Immutable;
using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Models;
using AutoCatalog.Application.Models.Extensions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Specs;

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

    public async Task<Result<List<Model>, Error>> GetAllAsync(ModelFilter filter, SortParameters sortParameters, PageParameters pageParameters, CancellationToken cancellationToken)
    {
        var models = await context.Models
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .ToListAsync(cancellationToken);
        return Result.Success<List<Model>, Error>(models);
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

    public async Task<Result<Unit, Error>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var modelResult = await GetByIdAsync(id, cancellationToken);
        if (modelResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(modelResult.Error);
        }

        var model = modelResult.Value;
        context.Models.Remove(model);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public async Task<Result<List<Model>, Error>> GetByBrandIdAsync(
        ModelFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        int brandId,
        CancellationToken ct)
    {
        var models = await context.Models
            .Where(m => m.BrandId == brandId)
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .ToListAsync(ct);

        return Result.Success<List<Model>, Error>(models);
    }
}