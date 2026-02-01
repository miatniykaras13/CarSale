using AutoCatalog.Application.AutoDriveTypes;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface IAutoDriveTypesRepository : IRepository<int, AutoDriveType, AutoDriveTypeFilter>
{
}