using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Application.Abstractions;

public interface ICarsRepository : IRepository<Guid, Car>
{
}