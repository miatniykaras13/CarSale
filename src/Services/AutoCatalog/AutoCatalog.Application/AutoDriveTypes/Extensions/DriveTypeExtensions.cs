using System.Linq.Expressions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.AutoDriveTypes.Extensions;

public static class AutoDriveTypeExtensions
{
    public static IQueryable<AutoDriveType> Filter(this IQueryable<AutoDriveType> query, AutoDriveTypeFilter filter)
    {
        return query;
    }

    public static IQueryable<AutoDriveType> Page(this IQueryable<AutoDriveType> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);


    public static IQueryable<AutoDriveType> Sort(this IQueryable<AutoDriveType> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));


    private static Expression<Func<AutoDriveType, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(AutoDriveType.Name) => x => x.Name,
            _ => x => x.Id
        };
    }
}