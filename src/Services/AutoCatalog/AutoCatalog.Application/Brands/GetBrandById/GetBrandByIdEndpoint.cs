using AutoCatalog.Application.Brands.GetBrands;
using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Brands.GetBrandById;

public record GetBrandByIdResponse(int Id, string Name, string Country, int YearFrom, int YearTo);

public class GetBrandByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/brands/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetBrandByIdQuery(id), ct);

            if (result.IsFailure)
                return result.Error.ToProblemDetails(context);

            var response = result.Value.Adapt<GetBrandsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBrandById")
        .Produces<GetBrandByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Brands")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get brand by id", Description = "Returns brand with given id" });
    }
}