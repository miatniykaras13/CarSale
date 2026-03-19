using AdService.Application.Commands.DeleteCarOption;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Delete;

public class DeleteCarOption : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapDelete("/car-options/{carOptionId:int}", async (
                HttpContext context,
                [FromRoute] int carOptionId,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new DeleteCarOptionCommand(carOptionId);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("DeleteCarOption")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags("CarOptions")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Delete car option",
                Description = "Deletes a car option by its identifier. " +
                              "Returns 409 Conflict if the option is still in use by any advertisement. " +
                              "Requires authorization.",
            });
}

