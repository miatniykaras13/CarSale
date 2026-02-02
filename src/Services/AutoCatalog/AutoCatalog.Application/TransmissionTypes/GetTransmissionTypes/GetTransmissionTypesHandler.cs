using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.TransmissionTypes.GetTransmissionTypes;

public record GetTransmissionTypesQuery(
    TransmissionTypeFilter Filter,
    SortParameters SortParameters,
    PageParameters PageParameters) : IQuery<Result<List<TransmissionType>, List<Error>>>;

public class GetTransmissionTypesQueryHandler(
    ITransmissionTypesRepository transmissionTypesRepository) : IQueryHandler<GetTransmissionTypesQuery, Result<List<TransmissionType>, List<Error>>>
{
    public async Task<Result<List<TransmissionType>, List<Error>>> Handle(
        GetTransmissionTypesQuery query,
        CancellationToken cancellationToken)
    {
        var transmissionTypeResult = await transmissionTypesRepository.GetAllAsync(
            query.Filter,
            query.SortParameters,
            query.PageParameters,
            cancellationToken);

        if (transmissionTypeResult.IsFailure)
            return Result.Failure<List<TransmissionType>, List<Error>>([transmissionTypeResult.Error]);

        var transmissionTypes = transmissionTypeResult.Value;
        return Result.Success<List<TransmissionType>, List<Error>>(transmissionTypes);
    }
}