using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Brands.PatchBrand;

public record PatchBrandCommand(int Id, string? Name, int? YearFrom, int? YearTo, string? Country)
    : ICommand<Result<int, List<Error>>>;

internal class PatchBrandCommandHandler(IBrandsRepository brandsRepository)
    : ICommandHandler<PatchBrandCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(PatchBrandCommand command, CancellationToken cancellationToken)
    {
        TypeAdapterConfig<PatchBrandCommand, Brand>
            .NewConfig()
            .IgnoreNullValues(true);

        var brandResult = await brandsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (brandResult.IsFailure)
            return Result.Failure<int, List<Error>>([brandResult.Error]);

        var brand = brandResult.Value;

        command.Adapt(brand);

        await brandsRepository.UpdateAsync(brand, cancellationToken);

        return Result.Success<int, List<Error>>(brand.Id);
    }
}