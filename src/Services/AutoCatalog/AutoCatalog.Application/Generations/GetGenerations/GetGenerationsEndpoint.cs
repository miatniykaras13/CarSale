using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Generations.GetGenerations;

public record GetGenerationResponse(int Id, string Name, int ModelId);

public class GetGenerationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/generations", async (ISender sender, CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetGenerationsQuery(), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetGenerationResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetGenerations")
        .Produces<List<GetGenerationResponse>>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Generations")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get generations", Description = "Returns the list of generations" });
    }
}