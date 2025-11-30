using AdService.Application.FileStorage;
using BuildingBlocks.CQRS;

namespace AdService.Application;

public record DeleteFileCommand(Guid FileId) : ICommand<bool>;

internal class DeleteFileCommandHandler(
    IFileStorage fileStorage) : ICommandHandler<DeleteFileCommand, bool>
{
    public async Task<bool> Handle(DeleteFileCommand command, CancellationToken ct = default)
    {
        var response = await fileStorage.DeleteFileAsync(command.FileId, ct);
        return response;
    }
}