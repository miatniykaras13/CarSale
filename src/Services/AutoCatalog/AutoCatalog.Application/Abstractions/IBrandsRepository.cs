using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface IBrandsRepository : IRepository<int, Brand>
{
}