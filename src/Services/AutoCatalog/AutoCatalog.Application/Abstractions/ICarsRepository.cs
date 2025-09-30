using AutoCatalog.Domain.Cars;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface ICarsRepository : IRepository<Guid, Car>
{
}