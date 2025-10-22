using AutoCatalog.Infrastructure;
using AutoCatalog.Web;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services.AddProgramDependencies(configuration);

services.AddSwaggerGenWithAuth(configuration);

services.AddAuthorization();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.Audience = configuration["Authentication:Audience"];
        o.MetadataAddress = configuration["Authentication:MetadataAddress"]!;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration["Authentication:ValidIssuer"],
        };
    });


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("autocatalog");
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auto catalog");
        options.RoutePrefix = string.Empty;
    });
}



app.UseExceptionHandler(options => { });

await app.UseAsyncSeeding();

app.UseHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    });

app.Run();