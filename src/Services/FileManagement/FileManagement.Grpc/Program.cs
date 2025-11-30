using FileManagement.Grpc;
using FileManagement.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var configuration = builder.Configuration;


builder.WebHost.ConfigureKestrel(o =>
{
    o.Limits.MaxRequestBodySize = null;
});


services.AddProgramDependencies(configuration);

var app = builder.Build();


app.MapGrpcService<FileService>();

app.Run();
