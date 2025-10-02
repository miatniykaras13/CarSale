using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Models.GetModelsByBrandId;

public record GetModelsByBrandIdResponse(int Id, string Name);

public class GetModelsByBrandIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/brands/{brandId:int}/models", async ([FromRoute] int brandId, ISender sender, CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetModelsByBrandIdQuery(brandId), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetModelsByBrandIdResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetModelsByBrandId")
        .Produces<GetModelsByBrandIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Models")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get models by brand id", Description = "Returns models with given brand id" });
    }
}