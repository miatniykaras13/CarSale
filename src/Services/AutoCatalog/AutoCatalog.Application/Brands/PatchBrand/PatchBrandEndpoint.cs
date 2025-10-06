using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Brands.PatchBrand;

public record PatchBrandRequest(string? Name, int? YearFrom, int? YearTo, string? Country);
public record PatchBrandResponse(int Id);

public class PatchBrandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/brands/{id:int}", async (
                [FromRoute] int id,
                [FromBody] PatchBrandRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new PatchBrandCommand(id, request.Name, request.YearFrom, request.YearTo, request.Country);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.NoContent();
            })
            .WithName("PatchBrand")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesPatchProblems()
            .WithTags("Brands")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Patch Brand", Description = "Returns brand id" });
    }
}