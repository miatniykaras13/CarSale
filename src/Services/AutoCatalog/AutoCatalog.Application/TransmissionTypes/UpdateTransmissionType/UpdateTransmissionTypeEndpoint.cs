using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.TransmissionTypes.UpdateTransmissionType;

public record UpdateTransmissionTypeRequest(string Name);

public record UpdateTransmissionTypeResponse(int Id);

public class UpdateTransmissionTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/transmission-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                [FromBody] UpdateTransmissionTypeRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateTransmissionTypeCommand(id, request.Name);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(new UpdateTransmissionTypeResponse(result.Value));
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("UpdateTransmissionType")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesUpdateProblems()
            .WithTags("TransmissionTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Update transmission type", Description = "Returns transmission type id" });
    }
}