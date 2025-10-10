using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Brands.DeleteBrand;

public record DeleteBrandCommand(int Id) : ICommand<Result<Unit, List<Error>>>;

public class DeleteBrandQueryHandler(IBrandsRepository brandsRepository)
    : ICommandHandler<DeleteBrandCommand, Result<Unit, List<Error>>>
{
    public async Task<Result<Unit, List<Error>>> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.DeleteAsync(command.Id, cancellationToken);
        if (brandResult.IsFailure)
            return Result.Failure<Unit, List<Error>>([brandResult.Error]);

        return Result.Success<Unit, List<Error>>(Unit.Value);
    }
}