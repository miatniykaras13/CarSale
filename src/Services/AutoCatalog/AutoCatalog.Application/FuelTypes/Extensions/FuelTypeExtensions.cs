using System.Linq.Expressions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.FuelTypes.Extensions;

public static class FuelTypeExtensions
{
    public static IQueryable<FuelType> Filter(this IQueryable<FuelType> query, FuelTypeFilter filter)
    {
        return query;
    }

    public static IQueryable<FuelType> Page(this IQueryable<FuelType> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);


    public static IQueryable<FuelType> Sort(this IQueryable<FuelType> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));


    private static Expression<Func<FuelType, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(FuelType.Name) => x => x.Name,
            _ => x => x.Id
        };
    }
}