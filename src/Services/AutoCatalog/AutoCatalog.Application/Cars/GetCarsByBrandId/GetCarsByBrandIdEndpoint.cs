using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Enums;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.GetCarsByBrandId;

public record GetCarsByBrandIdResponse(
    int Id,
    string Name,
    FuelType FuelType,
    float Volume,
    int HorsePower,
    int TorqueNm);

public class GetCarsByEngineIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/brands/{brandId:int}/cars", async ([FromRoute] int brandId, ISender sender, CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetCarsByBrandIdQuery(brandId), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetCarsByBrandIdResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetCarsByBrandId")
        .Produces<GetCarsByBrandIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Cars")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get cars by brand id", Description = "Returns cars with given brand id" });
    }
}