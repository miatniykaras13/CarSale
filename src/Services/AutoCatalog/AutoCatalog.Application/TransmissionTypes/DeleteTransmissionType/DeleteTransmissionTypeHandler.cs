using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.TransmissionTypes.DeleteTransmissionType;

public record DeleteTransmissionTypeCommand(int Id) : ICommand<UnitResult<List<Error>>>;

public class DeleteTransmissionTypeQueryHandler(ITransmissionTypesRepository transmissionTypesRepository)
    : ICommandHandler<DeleteTransmissionTypeCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(DeleteTransmissionTypeCommand command, CancellationToken cancellationToken)
    {
        var transmissionTypeResult = await transmissionTypesRepository.DeleteAsync(command.Id, cancellationToken);
        if (transmissionTypeResult.IsFailure)
            return UnitResult.Failure<List<Error>>([transmissionTypeResult.Error]);

        return UnitResult.Success<List<Error>>();
    }
}