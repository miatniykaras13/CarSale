using System.Linq.Expressions;
using AutoCatalog.Application.Models;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Abstractions.Repositories;

public interface IModelsRepository : IRepository<int, Model, ModelFilter>
{
}