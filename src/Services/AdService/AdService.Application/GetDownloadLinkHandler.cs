using AdService.Application.Abstractions.FileStorage;
using BuildingBlocks.CQRS;

namespace AdService.Application;

public record GetDownloadLinkQuery(Guid FileId, int ExpirySeconds) : IQuery<string>;

internal class GetDownloadLinkQueryHandler(
    IFileStorage fileStorage) : IQueryHandler<GetDownloadLinkQuery, string>
{
    public async Task<string> Handle(GetDownloadLinkQuery query, CancellationToken ct = default)
    {
        var response = await fileStorage.GetDownloadLinkAsync(query.FileId, query.ExpirySeconds, ct);
        return response;
    }
}