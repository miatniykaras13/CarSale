using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FileManagement.Grpc.Infra;

public class ImageProcessor : IImageProcessor
{
    public async Task<Stream> ResizeAsync(
        Stream stream,
        int targetWidth,
        int targetHeight,
        CancellationToken ct = default)
    {
        stream.Position = 0;
        using var image = await Image.LoadAsync<Rgba64>(stream, ct);

        int srcWidth = image.Width;
        int srcHeight = image.Height;

        float srcRatio = srcWidth / (float)srcHeight;
        float targetRatio = targetWidth / (float)targetHeight;

        if (Math.Abs(targetRatio - srcRatio) < 0.01f)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Stretch, Size = new Size(targetWidth, targetHeight),
            }));
        }
        else
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Crop,
                Position = AnchorPositionMode.Center,
                Size = new Size(targetWidth, targetHeight),
            }));
        }

        var ms = new MemoryStream();
        await image.SaveAsync(ms, image.Metadata.DecodedImageFormat!, ct);
        ms.Position = 0;
        return ms;
    }
}