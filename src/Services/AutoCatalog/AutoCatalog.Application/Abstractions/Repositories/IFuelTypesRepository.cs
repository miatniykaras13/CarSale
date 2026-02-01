using AutoCatalog.Application.FuelTypes;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface IFuelTypesRepository : IRepository<int, FuelType, FuelTypeFilter>
{
}