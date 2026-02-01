using System.Linq.Expressions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Engines.Extensions;

public static class EngineExtensions
{
    public static IQueryable<Engine> Filter(this IQueryable<Engine> query, EngineFilter filter)
    {
        if (filter.GenerationId != null)
            query = query.Where(e => e.GenerationId == filter.GenerationId);

        if (!string.IsNullOrEmpty(filter.GenerationName))
        {
            query = query.Where(e => e.Generation.Name.ToLower().Equals(filter.GenerationName.ToLower()));
        }

        if (filter.FuelTypeId != null)
            query = query.Where(e => e.FuelTypeId == filter.FuelTypeId);

        if (!string.IsNullOrEmpty(filter.FuelType))
            query = query.Where(e => e.FuelType.Name.ToLower().Equals(filter.FuelType.ToLower()));

        if (filter.TorqueNm != null)
            query = query.Where(e => e.TorqueNm == filter.TorqueNm);

        if (filter.Volume != null)
            query = query.Where(e => e.Volume.Equals(filter.Volume));

        if (filter.HorsePower != null)
            query = query.Where(e => e.HorsePower == filter.HorsePower);

        return query;
    }

    public static IQueryable<Engine> Page(this IQueryable<Engine> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);

    public static IQueryable<Engine> Sort(this IQueryable<Engine> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));

    private static Expression<Func<Engine, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(Engine.FuelType) => x => x.FuelType,
            nameof(Engine.HorsePower) => x => x.HorsePower,
            nameof(Engine.TorqueNm) => x => x.TorqueNm,
            nameof(Engine.Volume) => x => x.Volume,
            nameof(Engine.Name) => x => x.Name,
            _ => x => x.Id
        };
    }
}
