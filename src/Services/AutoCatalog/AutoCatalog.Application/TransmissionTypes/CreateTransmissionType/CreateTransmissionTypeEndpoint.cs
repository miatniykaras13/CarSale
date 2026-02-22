using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.TransmissionTypes.CreateTransmissionType;

public record CreateTransmissionTypeRequest(string Name);

public record CreateTransmissionTypeResponse(int Id);

public class CreateTransmissionTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/transmission-types", async (
                HttpContext context,
                CreateTransmissionTypeRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateTransmissionTypeCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                CreateTransmissionTypeResponse response = new(result.Value);
                return Results.Created($"/transmission-types/{response.Id}", response);
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("CreateTransmissionType")
            .Produces<CreateTransmissionTypeResponse>(StatusCodes.Status201Created)
            .ProducesPostProblems()
            .WithTags("TransmissionTypes")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create transmission type", Description = "Returns transmission type id" });
    }
}