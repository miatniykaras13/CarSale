using AdService.Application.Abstractions.FileStorage;
using AdService.Contracts.Files;
using BuildingBlocks.CQRS;

namespace AdService.Application;

public record GenerateThumbnailCommand(Guid FileId, ThumbnailDto ThumbnailDto) : ICommand<Guid>;

internal class GenerateThumbnailCommandHandler(
    IFileStorage fileStorage) : ICommandHandler<GenerateThumbnailCommand, Guid>
{
    public async Task<Guid> Handle(GenerateThumbnailCommand command, CancellationToken ct = default)
    {
        var response = await fileStorage.GenerateThumbnailAsync(command.FileId, command.ThumbnailDto, ct);
        return response;
    }
}