namespace AdService.Application.Commands.UploadImage;

public record UploadImageCommand(
    Guid AdId,
    Stream Stream,
    string FileName,
    string ContentType,
    Guid UserId) : ICommand<Result<Guid, List<Error>>>;