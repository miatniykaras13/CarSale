using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Repositories;

public class BrandsRepository : IBrandsRepository
{
    public Task<Result<Brand, Error>> GetByIdAsync(int id, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> AddAsync(Brand brand, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> UpdateAsync(Brand brand, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Result<int, Error>> DeleteAsync(Brand brand, CancellationToken cancellationToken) => throw new NotImplementedException();
}