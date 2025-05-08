using Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore;
using Serilog;
using Data.Repositories.Interfaces;
using Data.Models;
using Data.Repositories;
using BusinessLogic.Services;
using BusinessLogic.Services.Contracts;
using BusinessLogic.Mappers;
using Data.Hashers.Contracts;
using Data.Hashers;
using BusinessLogic.JWT;
using CarSaleApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();






builder.Services.AddApiAuthentication(builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>());

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddMvc(options => { options.SuppressAsyncSuffixInActionNames = false; });

var logger = new LoggerConfiguration().WriteTo.File("Logs/log.txt").CreateLogger();
builder.Host.UseSerilog(logger);


var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Sale API");
    });

}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
