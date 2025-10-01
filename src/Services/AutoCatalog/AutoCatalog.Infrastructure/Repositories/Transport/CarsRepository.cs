using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Transport.Cars;

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

    public async Task<Result<List<Car>, Error>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cars = await context.Cars.Include(c => c.Dimensions).ToListAsync(cancellationToken);
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
}