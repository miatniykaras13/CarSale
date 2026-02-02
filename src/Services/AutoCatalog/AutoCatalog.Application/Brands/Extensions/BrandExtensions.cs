using System.Linq.Expressions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Brands.Extensions;

public static class BrandExtensions
{
    public static IQueryable<Brand> Filter(this IQueryable<Brand> query, BrandFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Country))
            query = query.Where(x => x.Country.ToLower().Equals(filter.Country.ToLower()));

        if (filter.YearFrom.HasValue)
            query = query.Where(x => (x.YearTo ?? DateTime.UtcNow.Year) >= filter.YearFrom);

        if (filter.YearTo.HasValue)
            query = query.Where(x => x.YearFrom <= filter.YearTo);

        return query;
    }

    public static IQueryable<Brand> Page(this IQueryable<Brand> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);


    public static IQueryable<Brand> Sort(this IQueryable<Brand> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));


    private static Expression<Func<Brand, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(Brand.Name) => x => x.Name,
            nameof(Brand.Country) => x => x.Country,
            nameof(Brand.YearFrom) => x => x.YearFrom,
            nameof(Brand.YearTo) => x => x.YearTo ?? DateTime.UtcNow.Year,
            _ => x => x.Id
        };
    }
}