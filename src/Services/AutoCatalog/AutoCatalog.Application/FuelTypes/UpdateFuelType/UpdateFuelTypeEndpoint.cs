using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.FuelTypes.UpdateFuelType;

public record UpdateFuelTypeRequest(string Name);

public record UpdateFuelTypeResponse(int Id);

public class UpdateFuelTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/fuel-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                [FromBody] UpdateFuelTypeRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateFuelTypeCommand(id, request.Name);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(new UpdateFuelTypeResponse(result.Value));
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("UpdateFuelType")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesUpdateProblems()
            .WithTags("FuelTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Update transmission type", Description = "Returns transmission type id" });
    }
}