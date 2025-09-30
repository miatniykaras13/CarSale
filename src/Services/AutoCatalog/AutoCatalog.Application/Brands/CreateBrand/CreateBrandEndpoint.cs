using AutoCatalog.Application.Cars.CreateCar;
using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Brands.CreateBrand;

public record CreateBrandRequest(string Name, string Country, int YearFrom, int YearTo);

public record CreateBrandResponse(int Id);

public class CreateBrandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/brands", async (CreateBrandRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateBrandCommand>();

                var result = await sender.Send(command);

                if (result.IsFailure)
                    return result.ToResponse();

                CreateBrandResponse response = new(result.Value);
                return Results.Created($"/brands/{response.Id}", response);
            })
            .WithName("CreateBrand")
            .Produces<CreateBrandResponse>(StatusCodes.Status201Created)
            .ProducesPostProblems()
            .WithTags("Brands")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create brand", Description = "Returns brand id" });
    }
}