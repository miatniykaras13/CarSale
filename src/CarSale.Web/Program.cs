using CarSale.Application;
using CarSale.Application.Ads.Interfaces;
using CarSale.Web;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddProgramDependencies();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(string.Empty, options =>
    {
        options
            .WithTitle("CarSale API")
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch);
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();