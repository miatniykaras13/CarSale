using System.Linq.Expressions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.TransmissionTypes.Extensions;

public static class TransmissionTypeExtensions
{
    public static IQueryable<TransmissionType> Filter(this IQueryable<TransmissionType> query, TransmissionTypeFilter filter)
    {
        return query;
    }

    public static IQueryable<TransmissionType> Page(this IQueryable<TransmissionType> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);


    public static IQueryable<TransmissionType> Sort(this IQueryable<TransmissionType> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));


    private static Expression<Func<TransmissionType, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(TransmissionType.Name) => x => x.Name,
            _ => x => x.Id
        };
    }
}