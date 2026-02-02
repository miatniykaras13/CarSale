using System.Linq.Expressions;
using AutoCatalog.Application.Cars;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface ICarsRepository : IRepository<Guid, Car, CarFilter>
{
}