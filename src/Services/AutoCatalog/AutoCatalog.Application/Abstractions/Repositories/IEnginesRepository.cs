using System.Linq.Expressions;
using AutoCatalog.Application.Engines;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface IEnginesRepository : IRepository<int, Engine, EngineFilter>
{
}