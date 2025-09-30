using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Brands.GetBrands;

public record BrandDto(int Id, string Name, string Country, int YearFrom, int YearTo);
public record GetBrandsResponse(List<BrandDto> Brands);

public class GetBrandsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/brands", async (ISender sender) =>
        {
            var result = await sender.Send(new GetBrandsQuery());

            if (result.IsFailure)
                return result.ToResponse();

            var brandDtos = result.Value.Adapt<List<BrandDto>>();
            return Results.Ok(new GetBrandsResponse(brandDtos));
        })
        .WithName("GetBrands")
        .Produces<GetBrandsResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Brands")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get brands", Description = "Returns the list of brands" });
    }
}