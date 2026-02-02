using System.Linq.Expressions;
using AutoCatalog.Application.Generations;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface IGenerationsRepository : IRepository<int, Generation, GenerationFilter>
{
}