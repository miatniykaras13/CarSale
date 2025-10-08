using AutoCatalog.Infrastructure;
using AutoCatalog.Web;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services.AddProgramDependencies(configuration);

var app = builder.Build();

app.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalar();
}

app.UseExceptionHandler(options => { });

app.Run();