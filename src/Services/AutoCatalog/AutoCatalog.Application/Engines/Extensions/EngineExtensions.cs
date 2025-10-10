using System.Linq.Expressions;
using AutoCatalog.Application.Engines;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Engines.Extensions;

public static class EngineExtensions
{
    public static IQueryable<Engine> Filter(this IQueryable<Engine> query, EngineFilter filter)
    {
        if(filter.FuelType != null)
            query = query.Where(e => e.FuelType == filter.FuelType);
        if(filter.TorqueNm != null)
            query = query.Where(e => e.TorqueNm == filter.TorqueNm);
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