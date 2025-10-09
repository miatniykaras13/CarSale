using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Extensions;

public static class LoggerExtensions
{
    public static ILogger LogError(this ILogger logger, List<Error> error)
    {
        logger.LogError(
            "[ERROR] {ErrorType} errors occured with messages: {Errors} at the time {Time}",
            error[0].Type.ToString(),
            string.Join(',', error.Select(e => e.Message)),
            DateTime.UtcNow);
        return logger;
    }
}