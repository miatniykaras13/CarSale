using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.BodyTypes.CreateBodyType;

public record CreateBodyTypeCommand(string Name)
    : ICommand<Result<int, List<Error>>>;

internal class CreateBodyTypeCommandHandler(IBodyTypesRepository bodyTypesRepository)
    : ICommandHandler<CreateBodyTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateBodyTypeCommand command, CancellationToken cancellationToken)
    {
        BodyType bodyType = new()
        {
            Name = command.Name,
        };

        await bodyTypesRepository.AddAsync(bodyType, cancellationToken);

        return Result.Success<int, List<Error>>(bodyType.Id);
    }
}

