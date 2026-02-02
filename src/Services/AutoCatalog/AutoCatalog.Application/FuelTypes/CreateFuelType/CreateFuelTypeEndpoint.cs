using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.FuelTypes.CreateFuelType;

public record CreateFuelTypeRequest(string Name);

public record CreateFuelTypeResponse(int Id);

public class CreateFuelTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/fuel-types", async (
                HttpContext context,
                CreateFuelTypeRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateFuelTypeCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                CreateFuelTypeResponse response = new(result.Value);
                return Results.Created($"/fuel-types/{response.Id}", response);
            })
            .WithName("CreateFuelType")
            .Produces<CreateFuelTypeResponse>(StatusCodes.Status201Created)
            .ProducesPostProblems()
            .WithTags("FuelTypes")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create fuel type", Description = "Returns fuel type id" });
    }
}