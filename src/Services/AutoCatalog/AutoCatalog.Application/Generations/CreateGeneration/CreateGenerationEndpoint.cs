using AutoCatalog.Application.Generations.CreateGeneration;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Generations.CreateGeneration;

public record CreateGenerationRequest(int ModelId, string Name, int YearFrom, int? YearTo);

public record CreateGenerationResponse(int Id);

public class CreateGenerationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/generations", async (
                HttpContext context,
                CreateGenerationRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateGenerationCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                CreateGenerationResponse response = new(result.Value);
                return Results.Created($"/generations/{response.Id}", response);
            })
            .WithName("CreateGeneration")
            .Produces<CreateGenerationResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Generations")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create Generation", Description = "Returns generation id" });
    }
}