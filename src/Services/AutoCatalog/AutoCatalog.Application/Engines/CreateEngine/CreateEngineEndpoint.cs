using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Engines.CreateEngine;

public record CreateEngineRequest(
    int FuelTypeId,
    int GenerationId,
    string Name,
    float Volume,
    int HorsePower,
    int TorqueNm);

public record CreateEngineResponse(int Id);

public class CreateEngineEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/engines", async (
                HttpContext context,
                CreateEngineRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateEngineCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                CreateEngineResponse response = new(result.Value);
                return Results.Created($"/engines/{response.Id}", response);
            })
            .WithName("CreateEngine")
            .Produces<CreateEngineResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Engines")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create Engine", Description = "Returns engine id" });
    }
}