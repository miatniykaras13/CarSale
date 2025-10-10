using System.Collections.Immutable;
using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Cars;
using AutoCatalog.Application.Cars.Extensions;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Transport;

public class CarsRepository(AppDbContext context) : ICarsRepository
{
    public async Task<Result<Car, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var car = await context.Cars.FindAsync(id, cancellationToken);
        if (car == null)
        {
            return Result.Failure<Car, Error>(Error.NotFound(
                nameof(Car).ToLower(),
                $"Car with id {id} not found."));
        }

        return Result.Success<Car, Error>(car);
    }

    public async Task<Result<List<Car>, Error>> GetAllAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, CancellationToken cancellationToken)
    {
        var cars = await context.Cars
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .Include(c => c.Dimensions).ToListAsync(cancellationToken);
        return Result.Success<List<Car>, Error>(cars);
    }

    public async Task<Result<Guid, Error>> AddAsync(Car car, CancellationToken cancellationToken)
    {
        await context.Cars.AddAsync(car, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Guid, Error>(car.Id);
    }

    public async Task<Result<Guid, Error>> UpdateAsync(Car car, CancellationToken cancellationToken)
    {
        context.Cars.Update(car);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Guid, Error>(car.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var carResult = await GetByIdAsync(id, cancellationToken);
        if (carResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(carResult.Error);
        }

        var car = carResult.Value;
        context.Cars.Remove(car);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public async Task<Result<List<Car>, Error>> GetByBrandIdAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, int brandId, CancellationToken ct)
    {
        var cars = await context.Cars
            .Where(c => c.BrandId == brandId)
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .Include(c => c.Dimensions)
            .ToListAsync(ct);
        return Result.Success<List<Car>, Error>(cars);
    }

    public async Task<Result<List<Car>, Error>> GetByModelIdAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, int modelId, CancellationToken ct)
    {
        var cars = await context.Cars
            .Where(c => c.ModelId == modelId)
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .Include(c => c.Dimensions)
            .ToListAsync(ct);
        return Result.Success<List<Car>, Error>(cars);
    }

    public async Task<Result<List<Car>, Error>> GetByGenerationIdAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, int generationId, CancellationToken ct)
    {
        var cars = await context.Cars
            .Where(c => c.GenerationId == generationId)
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .Include(c => c.Dimensions)
            .ToListAsync(ct);
        return Result.Success<List<Car>, Error>(cars);
    }

    public async Task<Result<List<Car>, Error>> GetByEngineIdAsync(CarFilter filter, SortParameters sortParameters, PageParameters pageParameters, int engineId, CancellationToken ct)
    {
        var cars = await context.Cars
            .Where(c => c.EngineId == engineId)
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .Include(c => c.Dimensions)
            .ToListAsync(ct);
        return Result.Success<List<Car>, Error>(cars);
    }
}