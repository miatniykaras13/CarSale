using System.Text;

namespace AdService.Application.Builders;

public static class CacheKeyBuilder
{
    public static string Build(string type, params string?[] properties)
    {
        var key = new StringBuilder(type.ToLower());

        foreach (string? property in properties)
        {
            if (property != null)
                key.Append($":{property.ToLower()}");
        }

        return key.ToString();
    }
}