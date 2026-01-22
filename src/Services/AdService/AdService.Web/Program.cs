using AdService.Application;
using AdService.Contracts.Files;
using AdService.Web;
using AspNetCore.Swagger.Themes;
using Carter;
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

services.AddApiAuthentication(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(Theme.Dark, options =>
    {
        options.OAuthClientId("swagger-ad-service");
        options.OAuthUsePkce();
        options.OAuthScopes("openid", "profile");
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ad service");
        options.RoutePrefix = "docs";
    });
}

app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

await app.UseAsyncSeeding();

app.Run();