using System.Windows.Input;
using AdService.Application.FileStorage;
using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace AdService.Application;

public record UploadImageCommand(Stream Stream, string FileName, string ContentType) : ICommand<Guid>;

internal class UploadImageCommandHandler(
    IFileStorage fileStorage) : ICommandHandler<UploadImageCommand, Guid>
{
    public async Task<Guid> Handle(UploadImageCommand command, CancellationToken ct = default)
    {
        var fileId = await fileStorage.UploadImageAsync(command.Stream, command.FileName, command.ContentType, ct);
        return fileId;
    }
}