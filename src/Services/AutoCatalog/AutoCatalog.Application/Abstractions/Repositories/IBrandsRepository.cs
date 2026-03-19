using AutoCatalog.Application.Brands;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface IBrandsRepository : IRepository<int, Brand, BrandFilter>
{
    Task<Result<Brand, Error>> GetByName(string name, CancellationToken ct = default);
}