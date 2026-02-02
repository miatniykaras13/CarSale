using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.BodyTypes.DeleteBodyType;

public record DeleteBodyTypeCommand(int Id) : ICommand<UnitResult<List<Error>>>;

public class DeleteBodyTypeQueryHandler(IBodyTypesRepository bodyTypesRepository)
    : ICommandHandler<DeleteBodyTypeCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(DeleteBodyTypeCommand command, CancellationToken cancellationToken)
    {
        var bodyTypeResult = await bodyTypesRepository.DeleteAsync(command.Id, cancellationToken);
        if (bodyTypeResult.IsFailure)
            return UnitResult.Failure<List<Error>>([bodyTypeResult.Error]);

        return UnitResult.Success<List<Error>>();
    }
}