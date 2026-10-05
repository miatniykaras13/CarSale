using AspNetCore.Swagger.Themes;
using Carter;
using ProfileService.Web;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddProgramDependencies(configuration);
services.AddApiAuthentication(configuration);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(Theme.Dark, options =>
    {
        options.OAuthClientId(configuration["Keycloak:SwaggerClientId"]!);
        options.OAuthUsePkce();
        options.OAuthScopes("openid", "profile");
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Profile service");
        options.RoutePrefix = "docs";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

await app.UseAsyncMigrations();

app.Run();
