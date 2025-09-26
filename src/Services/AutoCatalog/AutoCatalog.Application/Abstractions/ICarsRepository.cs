using AutoCatalog.Domain.Cars;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface ICarsRepository
{
    Task<Result<Car, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> AddAsync(Car car, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> UpdateAsync(Car car, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteAsync(Car car, CancellationToken cancellationToken);
}