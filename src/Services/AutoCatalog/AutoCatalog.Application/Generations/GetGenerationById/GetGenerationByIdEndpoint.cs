using AutoCatalog.Application.Extensions;
using AutoCatalog.Application.Generations.GetGenerationById;
using AutoCatalog.Application.Generations.GetGenerations;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Generations.GetGenerationById;

public record GetGenerationByIdResponse(int Id, string Name, int ModelId);

public class GetGenerationByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/generations/{id:int}", async ([FromRoute] int id, ISender sender, CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetGenerationByIdQuery(id), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<GetGenerationResponse>();
            return Results.Ok(response);
        })
        .WithName("GetGenerationById")
        .Produces<GetGenerationByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Generations")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get generation by id", Description = "Returns generation with given id" });
    }
}