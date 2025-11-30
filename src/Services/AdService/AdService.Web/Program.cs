using AdService.Application;
using AdService.Contracts.Files;
using AdService.Web;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = long.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});


builder.WebHost.ConfigureKestrel(o =>
{
    o.Limits.MaxRequestBodySize = null;
});

services.AddProgramDependencies(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ad service");
        options.RoutePrefix = string.Empty;
    });
}


app.MapPost("/file", async ([FromForm] IFormFile file, ISender sender, CancellationToken ct = default) =>
    {
        var stream = file.OpenReadStream();

        var contentType = file.ContentType;

        var fileName = file.FileName;

        var command = new UploadLargeFileCommand(stream, fileName, contentType);

        var result = await sender.Send(command, ct);

        return Results.Ok(result);
    })
    .ExcludeFromDescription()
    .DisableAntiforgery();

app.MapGet("/file/{fileId:guid}", async (
        [FromRoute] Guid fileId,
        int expirySeconds,
        ISender sender,
        CancellationToken ct = default) =>
    {
        var request = new GetDownloadLinkQuery(fileId, expirySeconds);

        var response = await sender.Send(request, ct);
        return Results.Ok(response);
    })
    .ExcludeFromDescription()
    .DisableAntiforgery();

app.MapPost("/file/{fileId:guid}/thumbnail", async (
        [FromRoute] Guid fileId,
        [FromBody] ThumbnailDto thumbnailDto,
        ISender sender,
        CancellationToken ct = default) =>
    {
        var request = new GenerateThumbnailCommand(fileId, thumbnailDto);

        var response = await sender.Send(request, ct);
        return Results.Ok(response);
    })
    .ExcludeFromDescription()
    .DisableAntiforgery();

app.MapDelete("/image/{fileId:guid}", async (
        [FromRoute] Guid fileId,
        ISender sender,
        CancellationToken ct = default) =>
    {
        var request = new DeleteFileCommand(fileId);

        var success = await sender.Send(request, ct);

        if (!success)
            return Results.InternalServerError();

        return Results.NoContent();
    })
    .ExcludeFromDescription()
    .DisableAntiforgery();

app.UseHttpsRedirection();

await app.UseAsyncSeeding();

app.Run();