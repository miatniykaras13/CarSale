using System.Linq.Expressions;
using AutoCatalog.Application.Models;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Models.Extensions;

public static class ModelExtensions
{
    public static IQueryable<Model> Filter(this IQueryable<Model> query, ModelFilter filter)
    {
        return query;
    }

    public static IQueryable<Model> Page(this IQueryable<Model> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);


    public static IQueryable<Model> Sort(this IQueryable<Model> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));


    private static Expression<Func<Model, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(Model.Name) => x => x.Name,
            nameof(Model.BrandId) => x => x.BrandId,
            _ => x => x.Id
        };
    }
}