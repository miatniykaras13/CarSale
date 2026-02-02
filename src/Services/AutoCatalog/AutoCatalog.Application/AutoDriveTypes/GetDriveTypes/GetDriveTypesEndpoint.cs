using AutoCatalog.Application.Extensions;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.AutoDriveTypes.GetDriveTypes;

public record GetDriveTypesResponse(int Id, string Name);

public class GetDriveTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/drive-types", async (
                HttpContext context,
                [AsParameters] AutoDriveTypeFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new GetDriveTypesQuery(filter, sortParameters, pageParameters), ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                var response = result.Value.Adapt<List<GetDriveTypesResponse>>();
                return Results.Ok(response);
            })
            .WithName("GetDriveTypes")
            .Produces<List<GetDriveTypesResponse>>(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("DriveTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Get drive types", Description = "Returns the list of drive types" });
    }
}