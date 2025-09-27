using AutoCatalog.Domain.Cars;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface IBrandsRepository
{
    Task<Result<Brand, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Result<int, Error>> AddAsync(Brand brand, CancellationToken cancellationToken);

    Task<Result<int, Error>> UpdateAsync(Brand brand, CancellationToken cancellationToken);

    Task<Result<int, Error>> DeleteAsync(int id, CancellationToken cancellationToken);
}