using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.AutoDriveTypes.GetDriveTypeById;

public record GetDriveTypeByIdResponse(int Id, string Name);

public class GetDriveTypeByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/drive-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetDriveTypeByIdQuery(id), ct);

            if (result.IsFailure)
                return result.Error.ToProblemDetails(context);

            var response = result.Value.Adapt<GetDriveTypeByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetDriveTypeById")
        .Produces<GetDriveTypeByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("DriveTypes")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get drive type by id", Description = "Returns drive type with given id" });
    }
}