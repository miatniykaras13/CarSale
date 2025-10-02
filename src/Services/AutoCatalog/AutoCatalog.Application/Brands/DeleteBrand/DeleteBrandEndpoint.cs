using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Brands.DeleteBrand;

public class DeleteBrandsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/brands/{id:int}", async ([FromRoute] int id, ISender sender, CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteBrandCommand(id), ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.NoContent();
            })
            .WithName("DeleteBrands")
            .Produces(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("Brands")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete brands", Description = "Deletes brand by id" });
    }
}