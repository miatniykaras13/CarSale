using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Brands.DeleteBrand;

public record DeleteBrandRequest(int Id);

public class DeleteBrandsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/brands", async (DeleteBrandRequest request, ISender sender, CancellationToken ct = default) =>
            {
                var command = request.Adapt<DeleteBrandCommand>();
                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.Ok();
            })
            .WithName("DeleteBrands")
            .Produces(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("Brands")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete brands", Description = "Deletes brand by id" });
    }
}