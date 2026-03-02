using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record AdImage
{
    private List<Thumbnail> _thumbnails = [];

    public Guid Id { get; private set; }

    public IReadOnlyList<Thumbnail> Thumbnails => _thumbnails.AsReadOnly();

    protected AdImage()
    {
    }

    private AdImage(Guid id)
    {
        Id = id;
    }

    public static Result<AdImage, Error> Of(Guid id)
    {
        var image = new AdImage(id);
        return image;
    }

    public UnitResult<Error> AddThumbnail(Thumbnail thumbnail)
    {
        if (_thumbnails.Contains(thumbnail))
        {
            return UnitResult.Failure(Error.Domain(
                "image.thumbnail.already_exist",
                "Image thumbnail already exist"));
        }

        _thumbnails.Add(thumbnail);
        return UnitResult.Success<Error>();
    }
}