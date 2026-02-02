using AutoCatalog.Application.Extensions;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.TransmissionTypes.GetTransmissionTypes;

public record GetTransmissionTypesResponse(int Id, string Name);

public class GetTransmissionTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/transmission-types", async (
                [AsParameters] TransmissionTypeFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new GetTransmissionTypesQuery(filter, sortParameters, pageParameters), ct);

                if (result.IsFailure)
                    return result.Error.ToResponse();

                var response = result.Value.Adapt<List<GetTransmissionTypesResponse>>();
                return Results.Ok(response);
            })
            .WithName("GetTransmissionTypes")
            .Produces<List<GetTransmissionTypesResponse>>(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("TransmissionTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Get transmission types", Description = "Returns the list of transmission types" });
    }
}