using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Enums;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Engines.GetEnginesByGenerationId;

public record GetEnginesByGenerationIdResponse(
    int Id,
    string Name,
    FuelType FuelType,
    float Volume,
    int HorsePower,
    int TorqueNm);

public class GetEnginesByGenerationIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/generations/{generationId:int}/engines", async (
                [AsParameters] EngineFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                [FromRoute] int generationId,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetEnginesByGenerationIdQuery(filter, sortParameters, pageParameters, generationId), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetEnginesByGenerationIdResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetEnginesByGenerationId")
        .Produces<GetEnginesByGenerationIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Engines")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get engines by generation id", Description = "Returns engines with given generation id" });
    }
}