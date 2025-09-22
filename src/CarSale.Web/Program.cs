using CarSale.Application;
using CarSale.Application.Ads.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddProgramDependencies();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "CarSale");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();