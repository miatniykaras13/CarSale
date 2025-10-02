using AutoCatalog.Application.Abstractions;

namespace AutoCatalog.Application.Brands.PatchBrand;

public record PatchBrandCommand(int Id, string? Name, int? YearFrom, int? YearTo, string? Country)
    : ICommand<Result<int, List<Error>>>;

internal class PatchBrandCommandHandler(IBrandsRepository generationsRepository)
    : ICommandHandler<PatchBrandCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(PatchBrandCommand command, CancellationToken cancellationToken)
    {
        var generationResult = await generationsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<int, List<Error>>([generationResult.Error]);

        var generation = generationResult.Value;

        command.Adapt(generationResult.Value);

        await generationsRepository.AddAsync(generation, cancellationToken);

        return Result.Success<int, List<Error>>(generation.Id);
    }
}