using System.Windows.Input;
using AdService.Application.FileStorage;
using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace AdService.Application;

public record UploadLargeFileCommand(Stream Stream, string FileName, string ContentType) : ICommand<Guid>;

internal class UploadLargeFileCommandHandler(
    IFileStorage fileStorage) : ICommandHandler<UploadLargeFileCommand, Guid>
{
    public async Task<Guid> Handle(UploadLargeFileCommand command, CancellationToken ct = default)
    {
        var fileId = await fileStorage.UploadLargeFileAsync(command.Stream, command.FileName, command.ContentType, ct);
        return fileId;
    }
}