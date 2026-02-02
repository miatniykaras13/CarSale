using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.TransmissionTypes.CreateTransmissionType;

public record CreateTransmissionTypeCommand(string Name)
    : ICommand<Result<int, List<Error>>>;

internal class CreateTransmissionTypeCommandHandler(ITransmissionTypesRepository transmissionTypesRepository)
    : ICommandHandler<CreateTransmissionTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateTransmissionTypeCommand command, CancellationToken cancellationToken)
    {
        TransmissionType transmissionType = new()
        {
            Name = command.Name,
        };

        await transmissionTypesRepository.AddAsync(transmissionType, cancellationToken);

        return Result.Success<int, List<Error>>(transmissionType.Id);
    }
}

