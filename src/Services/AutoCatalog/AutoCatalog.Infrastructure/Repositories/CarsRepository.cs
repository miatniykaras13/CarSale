using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Cars;

namespace AutoCatalog.Infrastructure.Repositories;

public class CarsRepository : ICarsRepository
{
    public Task<Result<Car, Error>> GetByIdAsync(int id, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<Guid, Error>> AddAsync(Car car, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<Guid, Error>> UpdateAsync(Car car, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<Guid, Error>> DeleteAsync(Car car, CancellationToken cancellationToken) => throw new NotImplementedException();
}