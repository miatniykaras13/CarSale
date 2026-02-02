using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.FuelTypes.DeleteFuelType;

public class DeleteFuelTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/fuel-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteFuelTypeCommand(id), ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                return Results.NoContent();
            })
            .WithName("DeleteFuelType")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesDeleteProblems()
            .WithTags("FuelTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete fuel type", Description = "Deletes fuel type by id" });
    }
}