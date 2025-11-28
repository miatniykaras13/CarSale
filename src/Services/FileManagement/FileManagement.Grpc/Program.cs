using FileManagement.Grpc;
using FileManagement.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var configuration = builder.Configuration;

builder.Services.AddGrpc(options =>
{
    options.MaxReceiveMessageSize = null; // снимаем лимит
    options.MaxSendMessageSize = null;
});
builder.WebHost.ConfigureKestrel(o =>
{
    o.Limits.MaxRequestBodySize = null; // снимаем глобально
});

// Add services to the container.
services.AddProgramDependencies(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<FileService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
