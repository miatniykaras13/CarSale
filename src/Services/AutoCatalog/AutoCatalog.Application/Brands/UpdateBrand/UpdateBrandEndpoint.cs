using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Brands.UpdateBrand;

public record UpdateBrandRequest(string Name, int YearFrom, int? YearTo, string Country);

public record UpdateBrandResponse(int Id);

public class UpdateBrandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/brands/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                [FromBody] UpdateBrandRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateBrandCommand(id, request.Name, request.Country, request.YearFrom, request.YearTo);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(new UpdateBrandResponse(result.Value));
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("UpdateBrand")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesUpdateProblems()
            .WithTags("Brands")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Update Brand", Description = "Returns brand id" });
    }
}