using AutoCatalog.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IServiceCollection services = builder.Services;

services.AddProgramDependencies(configuration);

WebApplication app = builder.Build();

app.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalar();
}


app.Run();