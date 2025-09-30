using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface IModelsRepository : IRepository<int, Model>
{
}