using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.TransmissionTypes.GetTransmissionTypeById;

public record GetTransmissionTypeByIdQuery(int Id) : IQuery<Result<TransmissionType, List<Error>>>;

public class GetTransmissionTypeByIdQueryHandler(ITransmissionTypesRepository transmissionTypesRepository)
    : IQueryHandler<GetTransmissionTypeByIdQuery, Result<TransmissionType, List<Error>>>
{
    public async Task<Result<TransmissionType, List<Error>>> Handle(
        GetTransmissionTypeByIdQuery query,
        CancellationToken cancellationToken)
    {
        var transmissionTypeResult = await transmissionTypesRepository.GetByIdAsync(query.Id, cancellationToken);
        if (transmissionTypeResult.IsFailure)
            return Result.Failure<TransmissionType, List<Error>>([transmissionTypeResult.Error]);

        var transmissionType = transmissionTypeResult.Value;
        return Result.Success<TransmissionType, List<Error>>(transmissionType);
    }
}