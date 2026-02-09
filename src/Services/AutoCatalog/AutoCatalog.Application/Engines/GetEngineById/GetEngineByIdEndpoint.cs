using AutoCatalog.Application.Engines.Dtos;
using AutoCatalog.Application.Engines.GetEngines;
using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Engines.GetEngineById;

public record GetEngineByIdResponse(
    int Id,
    int GenerationId,
    string Name,
    FuelTypeDto FuelType,
    float Volume,
    int HorsePower,
    int TorqueNm);

public class GetEngineByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/engines/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetEngineByIdQuery(id), ct);

            if (result.IsFailure)
                return result.Error.ToProblemDetails(context);

            var response = result.Value.Adapt<GetEngineResponse>();
            return Results.Ok(response);
        })
        .WithName("GetEngineById")
        .Produces<GetEngineByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Engines")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get engine by id", Description = "Returns engine with given id" });
    }
}