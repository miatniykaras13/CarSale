using System.Linq.Expressions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Cars;
using AutoCatalog.Application.Cars.Extensions;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Transport;

public class CarsRepository(AppDbContext context) : ICarsRepository
{
    public async Task<Result<Car, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var car = await context.Cars
            .AsNoTracking()
            .Include(c => c.Brand)
            .Include(c => c.Model)
            .Include(c => c.Generation)
            .Include(c => c.Engine).ThenInclude(e => e.FuelType)
            .Include(c => c.TransmissionType)
            .Include(c => c.DriveType)
            .Include(c => c.BodyType)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (car == null)
        {
            return Result.Failure<Car, Error>(Error.NotFound(
                nameof(Car).ToLower(),
                $"Car with id {id} not found."));
        }

        return Result.Success<Car, Error>(car);
    }

    public async Task<Result<List<Car>, Error>> GetAllAsync(
        CarFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var cars = await context.Cars
            .AsNoTracking()
            .Include(c => c.Brand)
            .Include(c => c.Model)
            .Include(c => c.Generation)
            .Include(c => c.Engine).ThenInclude(e => e.FuelType)
            .Include(c => c.TransmissionType)
            .Include(c => c.DriveType)
            .Include(c => c.BodyType)
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .ToListAsync(cancellationToken);
        return Result.Success<List<Car>, Error>(cars);
    }

    public async Task<Result<Guid, Error>> AddAsync(Car car, CancellationToken cancellationToken = default)
    {
        await context.Cars.AddAsync(car, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Guid, Error>(car.Id);
    }

    public async Task<Result<Guid, Error>> UpdateAsync(Car car, CancellationToken cancellationToken = default)
    {
        context.Cars.Update(car);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Guid, Error>(car.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
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
}