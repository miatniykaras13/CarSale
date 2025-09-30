using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Abstractions;

public interface IEnginesRepository : IRepository<int, Engine>
{
}