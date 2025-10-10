using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Models.PatchModel;

public record PatchModelCommand(int Id, string? Name)
    : ICommand<Result<int, List<Error>>>;

internal class PatchModelCommandHandler(IModelsRepository modelsRepository)
    : ICommandHandler<PatchModelCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(PatchModelCommand command, CancellationToken cancellationToken)
    {
        TypeAdapterConfig<PatchModelCommand, Model>
            .NewConfig()
            .IgnoreNullValues(true);

        var modelResult = await modelsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (modelResult.IsFailure)
            return Result.Failure<int, List<Error>>([modelResult.Error]);

        var model = modelResult.Value;

        command.Adapt(model);

        await modelsRepository.AddAsync(model, cancellationToken);

        return Result.Success<int, List<Error>>(model.Id);
    }
}