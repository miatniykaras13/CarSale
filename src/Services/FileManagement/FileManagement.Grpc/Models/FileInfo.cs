namespace FileManagement.Grpc.Models;

public sealed class FileInfo
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Extension { get; private set; }

    public string ContentType { get; private set; }

    public long Size { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private FileInfo(Guid id, string name, long size, string extension, DateTime createdAt, string contentType)
    {
        Id = id;
        Size = size;
        Extension = extension;
        Name = name;
        CreatedAt = createdAt;
        ContentType = contentType;
    }

    public static FileInfo Create(Guid id, string name, long size, string extension, DateTime createdAt,
        string contentType)
    {
        FileInfo info = new(id, name, size, extension, createdAt, contentType);
        return info;
    }
}