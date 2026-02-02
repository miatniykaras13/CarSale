using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.FuelTypes.GetFuelTypeById;

public record GetFuelTypeByIdResponse(int Id, string Name);

public class GetFuelTypeByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/fuel-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetFuelTypeByIdQuery(id), ct);

            if (result.IsFailure)
                return result.Error.ToResponse(context);

            var response = result.Value.Adapt<GetFuelTypeByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetFuelTypeById")
        .Produces<GetFuelTypeByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("FuelTypes")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get fuel type by id", Description = "Returns fuel type with given id" });
    }
}