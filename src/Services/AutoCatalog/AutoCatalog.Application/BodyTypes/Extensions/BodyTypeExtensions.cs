using System.Linq.Expressions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.BodyTypes.Extensions;

public static class BodyTypeExtensions
{
    public static IQueryable<BodyType> Filter(this IQueryable<BodyType> query, BodyTypeFilter filter)
    {
        return query;
    }

    public static IQueryable<BodyType> Page(this IQueryable<BodyType> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);


    public static IQueryable<BodyType> Sort(this IQueryable<BodyType> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));


    private static Expression<Func<BodyType, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(BodyType.Name) => x => x.Name,
            _ => x => x.Id
        };
    }
}