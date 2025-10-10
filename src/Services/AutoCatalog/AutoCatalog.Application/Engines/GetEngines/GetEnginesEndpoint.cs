using AutoCatalog.Application.Engines.GetEngines;
using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Enums;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Engines.GetEngines;

public record GetEngineResponse(
    int Id,
    int GenerationId,
    string Name,
    FuelType FuelType,
    float Volume,
    int HorsePower,
    int TorqueNm);

public class GetEnginesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/engines", async (
                [AsParameters] EngineFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetEnginesQuery(filter, sortParameters, pageParameters), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetEngineResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetEngines")
        .Produces<List<GetEngineResponse>>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Engines")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get engines", Description = "Returns the list of engines" });
    }
}