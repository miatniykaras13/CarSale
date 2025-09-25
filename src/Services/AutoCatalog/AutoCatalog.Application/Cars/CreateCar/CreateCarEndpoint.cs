using AutoCatalog.Domain.Enums;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoCatalog.Application.Cars.CreateCar;

public record CreateCarRequest(
    int BrandId,
    int ModelId,
    int GenerationId,
    int EngineId,
    TransmissionType TransmissionType,
    AutoDriveType AutoDriveType,
    int YearFrom,
    int YearTo,
    Guid PhotoId,
    int Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto);

public record CreateCarResponse(Guid Id);

public class CreateCarEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/cars", async (CreateCarRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateCarCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateCarResponse>();
                return Results.Created($"/cars/{response.Id}", response);
            })
            .WithName("CreateCar")
            .Produces<CreateCarResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Cars")
            .WithOpenApi(op => new(op) { Summary = "Create car", Description = "Returns car id", });
    }
}