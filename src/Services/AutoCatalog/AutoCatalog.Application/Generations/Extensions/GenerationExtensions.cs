﻿using System.Linq.Expressions;
using AutoCatalog.Application.Generations;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Generations.Extensions;

public static class GenerationExtensions
{
    public static IQueryable<Generation> Filter(this IQueryable<Generation> query, GenerationFilter filter)
    {
        return query;
    }

    public static IQueryable<Generation> Page(this IQueryable<Generation> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);


    public static IQueryable<Generation> Sort(this IQueryable<Generation> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));


    private static Expression<Func<Generation, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(Generation.Name) => x => x.Name,
            _ => x => x.Id
        };
    }
}