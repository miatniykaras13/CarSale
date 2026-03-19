using AutoCatalog.Application.Cars.DeleteCar;
using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.DeleteImage;

public class DeleteImageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/cars/{carId:guid}/image", async (
                HttpContext context,
                [FromRoute] Guid carId,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteImageCommand(carId), ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("DeleteImage")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesDeleteProblems()
            .WithTags("Cars")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete image", Description = "Deletes car's image by id" });
    }
}