using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Models.UpdateModel;

public record UpdateModelRequest(int BrandId, string Name);
public record UpdateModelResponse(int Id);

public class UpdateModelEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/models/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                [FromBody] UpdateModelRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateModelCommand(id, request.BrandId, request.Name);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .WithName("UpdateModel")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesUpdateProblems()
            .WithTags("Models")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Update Model", Description = "Returns model id" });
    }
}