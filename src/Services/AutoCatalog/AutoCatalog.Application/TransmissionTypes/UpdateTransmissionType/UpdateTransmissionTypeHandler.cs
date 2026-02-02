using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.TransmissionTypes.UpdateTransmissionType;

public record UpdateTransmissionTypeCommand(int Id, string Name)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateTransmissionTypeCommandHandler(ITransmissionTypesRepository transmissionTypesRepository)
    : ICommandHandler<UpdateTransmissionTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(UpdateTransmissionTypeCommand command, CancellationToken cancellationToken)
    {
        var transmissionTypeResult = await transmissionTypesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (transmissionTypeResult.IsFailure)
            return Result.Failure<int, List<Error>>([transmissionTypeResult.Error]);

        var transmissionType = transmissionTypeResult.Value;

        command.Adapt(transmissionType);

        await transmissionTypesRepository.UpdateAsync(transmissionType, cancellationToken);

        return Result.Success<int, List<Error>>(transmissionType.Id);
    }
}