using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.Models.UpdateModel;

public record UpdateModelCommand(int Id, int BrandId, string Name)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateModelCommandHandler(IModelsRepository modelsRepository)
    : ICommandHandler<UpdateModelCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(UpdateModelCommand command, CancellationToken cancellationToken)
    {
        var modelResult = await modelsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (modelResult.IsFailure)
            return Result.Failure<int, List<Error>>([modelResult.Error]);

        var model = modelResult.Value;

        if (model.BrandId != command.BrandId)
        {
            return Result.Failure<int, List<Error>>(Error.Validation(
                "brand_id",
                "Cannot change brand id. Recreate the model"));
        }

        command.Adapt(model);

        await modelsRepository.AddAsync(model, cancellationToken);

        return Result.Success<int, List<Error>>(model.Id);
    }
}