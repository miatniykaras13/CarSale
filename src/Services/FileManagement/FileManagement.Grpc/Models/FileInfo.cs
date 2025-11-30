namespace FileManagement.Grpc.Models;

public sealed class FileInfo
{
    public Guid Id { get; private set; }

    public Guid? ParentId { get; private set; }

    public string Name { get; private set; }

    public string Extension { get; private set; }

    public string ContentType { get; private set; }

    public long Size { get; private set; }

    public bool IsThumbnail => ParentId is not null;

    public DateTime CreatedAt { get; private set; }

    public FileInfo? Parent { get; private set; }

    public List<FileInfo>? Children { get; private set; } = [];

    protected FileInfo()
    {
    }

    private FileInfo(
        Guid id,
        string name,
        long size,
        string extension,
        DateTime createdAt,
        string contentType,
        Guid? parentId)
    {
        Id = id;
        Name = name;
        Size = size;
        Extension = extension;
        CreatedAt = createdAt;
        ContentType = contentType;
        ParentId = parentId;
    }

    public static FileInfo Create(
        Guid id,
        string name,
        long size,
        string extension,
        DateTime createdAt,
        string contentType,
        Guid? parentId = null)
    {
        FileInfo info = new(id, name, size, extension, createdAt, contentType, parentId);
        return info;
    }
}