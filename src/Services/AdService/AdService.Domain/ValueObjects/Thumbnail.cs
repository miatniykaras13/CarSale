using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record Thumbnail
{
    public Guid Id { get; private set; }

    public Guid AdImageId { get; private set; }

    public AdImage? AdImage { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    private Thumbnail(
        Guid id,
        Guid adImageId,
        int width,
        int height)
    {
        Id = id;
        AdImageId = adImageId;
        Width = width;
        Height = height;
    }

    protected Thumbnail()
    {
    }

    public static Result<Thumbnail, Error> Of(
        Guid id,
        Guid adImageId,
        int width,
        int height)
    {
        var thumbnail = new Thumbnail(id, adImageId, width, height);
        return thumbnail;
    }
}