using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AutoCatalog.Application.Extensions;

public static class CarterModuleExtensions
{
    public static RouteHandlerBuilder ProducesGetProblems(this RouteHandlerBuilder builder)
    {
        builder
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        return builder;
    }

    public static RouteHandlerBuilder ProducesPostProblems(this RouteHandlerBuilder builder)
    {
        builder
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        return builder;
    }

    public static RouteHandlerBuilder ProducesDeleteProblems(this RouteHandlerBuilder builder)
    {
        builder
            .ProducesAuthorizationProblems()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        return builder;
    }

    public static RouteHandlerBuilder ProducesUpdateProblems(this RouteHandlerBuilder builder)
    {
        builder
            .ProducesAuthorizationProblems()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        return builder;
    }

    public static RouteHandlerBuilder ProducesAuthorizationProblems(this RouteHandlerBuilder builder)
    {
        builder
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);
        return builder;
    }
}