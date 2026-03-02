using AdService.Application.Commands.DeleteCarOption;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
            .Produces(StatusCodes.Status409Conflict);
}

