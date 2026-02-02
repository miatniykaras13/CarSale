using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.TransmissionTypes.GetTransmissionTypeById;

public record GetTransmissionTypeByIdResponse(int Id, string Name);

public class GetTransmissionTypeByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/transmission-types/{id:int}", async (
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetTransmissionTypeByIdQuery(id), ct);

            if (result.IsFailure)
                return result.Error.ToResponse();

            var response = result.Value.Adapt<GetTransmissionTypeByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetTransmissionTypeById")
        .Produces<GetTransmissionTypeByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("TransmissionTypes")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get transmission type by id", Description = "Returns transmission type with given id" });
    }
}