namespace FileManagement.Grpc.Models;

public sealed class FileInfo
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Extension { get; private set; }

    public long Size { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private FileInfo(Guid id, string name, long size, string extension, DateTime createdAt)
    {
        Id = id;
        Size = size;
        Extension = extension;
        Name = name;
        CreatedAt = createdAt;
    }

    public static FileInfo Create(Guid id, string name, long size, string extension, DateTime createdAt)
    {
        FileInfo info = new(id, name, size, extension,  createdAt);
        return info;
    }
}