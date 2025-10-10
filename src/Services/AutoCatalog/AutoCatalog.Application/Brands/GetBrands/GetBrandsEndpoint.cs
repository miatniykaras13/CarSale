﻿using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Brands.GetBrands;

public record GetBrandResponse(int Id, string Name, string Country, int YearFrom, int YearTo);

public class GetBrandsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/brands", async (
                [AsParameters] BrandFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new GetBrandsQuery(filter, sortParameters, pageParameters), ct);

                if (result.IsFailure)
                    return result.ToResponse();

                var response = result.Value.Adapt<List<GetBrandResponse>>();
                return Results.Ok(response);
            })
            .WithName("GetBrands")
            .Produces<List<GetBrandResponse>>(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("Brands")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Get brands", Description = "Returns the list of brands" });
    }
}