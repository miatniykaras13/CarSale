using FileManagement.Grpc.Data;

namespace FileManagement.Grpc;

public static class ApiExtensions
{
    public static async Task<WebApplication> UseMigrating(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<FileManagementDbContext>();
        await context.Database.EnsureCreatedAsync();
        return app;
    }
}