using System.Security.Claims;
using AdService.Application.Commands.CreateAd;
using AdService.Application.Commands.MergePatchAdsCar;
using AdService.Contracts.Cars;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class MergePatchAdsCar : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPatch("/ads/{adId:guid}/car", async (
                [FromRoute] Guid adId,
                [FromBody] CarSnapshotMergePatchDto dto,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new MergePatchAdsCarCommand(adId, dto);

                var result = await sender.Send(command, ct);
                if (result.IsFailure)
                    return result.Error.ToResponse();


                return Results.Ok();
            })
            .WithName("MergePatchAdsCar")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK);
}