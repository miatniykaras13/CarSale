using AutoCatalog.Application.BodyTypes;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface IBodyTypesRepository : IRepository<int, BodyType, BodyTypeFilter>
{
}