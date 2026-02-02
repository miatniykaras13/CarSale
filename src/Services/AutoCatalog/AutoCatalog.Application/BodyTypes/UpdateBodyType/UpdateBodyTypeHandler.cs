using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.BodyTypes.UpdateBodyType;

public record UpdateBodyTypeCommand(int Id, string Name)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateBodyTypeCommandHandler(IBodyTypesRepository bodyTypesRepository)
    : ICommandHandler<UpdateBodyTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(UpdateBodyTypeCommand command, CancellationToken cancellationToken)
    {
        var bodyTypeResult = await bodyTypesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (bodyTypeResult.IsFailure)
            return Result.Failure<int, List<Error>>([bodyTypeResult.Error]);

        var bodyType = bodyTypeResult.Value;

        command.Adapt(bodyType);

        await bodyTypesRepository.UpdateAsync(bodyType, cancellationToken);

        return Result.Success<int, List<Error>>(bodyType.Id);
    }
}