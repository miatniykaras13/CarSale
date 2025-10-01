using AutoCatalog.Application.Models.CreateModel;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Models.CreateModel;

public record CreateModelRequest(int BrandId, string Name);

public record CreateModelResponse(int Id);

public class CreateModelEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/models", async (CreateModelRequest request, ISender sender, CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateModelCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                CreateModelResponse response = new(result.Value);
                return Results.Created($"/models/{response.Id}", response);
            })
            .WithName("CreateModel")
            .Produces<CreateModelResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Models")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create Model", Description = "Returns model id" });
    }
}