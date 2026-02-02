using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.BodyTypes.CreateBodyType;

public record CreateBodyTypeRequest(string Name);

public record CreateBodyTypeResponse(int Id);

public class CreateBodyTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/body-types", async (
                CreateBodyTypeRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateBodyTypeCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToResponse();

                CreateBodyTypeResponse response = new(result.Value);
                return Results.Created($"/body-types/{response.Id}", response);
            })
            .WithName("CreateBodyType")
            .Produces<CreateBodyTypeResponse>(StatusCodes.Status201Created)
            .ProducesPostProblems()
            .WithTags("BodyTypes")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create body type", Description = "Returns body type id" });
    }
}