using AdService.Domain.Entities;
using BuildingBlocks.Application.Paging;

namespace AdService.Application.Queries.GetAllCarOptions;

public static class CarOptionExtensions
{
    public static IQueryable<CarOption> Filter(this IQueryable<CarOption> query, CarOptionFilter filter)
    {
        if (filter.OptionType is not null)
            query = query.Where(o => o.OptionType.ToString().ToLower().Equals(filter.OptionType.ToLower()));

        return query;
    }

    public static IQueryable<CarOption> Page(this IQueryable<CarOption> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);
}

