using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.AutoDriveTypes.CreateDriveType;

public record CreateDriveTypeRequest(string Name);

public record CreateDriveTypeResponse(int Id);

public class CreateDriveTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/drive-types", async (
                HttpContext context,
                CreateDriveTypeRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateDriveTypeCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                CreateDriveTypeResponse response = new(result.Value);
                return Results.Created($"/drive-types/{response.Id}", response);
            })
            .WithName("CreateDriveType")
            .Produces<CreateDriveTypeResponse>(StatusCodes.Status201Created)
            .ProducesPostProblems()
            .WithTags("DriveTypes")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create drive type", Description = "Returns drive type id" });
    }
}