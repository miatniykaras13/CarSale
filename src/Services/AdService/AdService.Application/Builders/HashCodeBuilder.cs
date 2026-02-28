using System.Security.Cryptography;
using System.Text;

namespace AdService.Application.Builders;

/// <summary>
/// Constructs a deterministic hash string from an arbitrary set of values using SHA256.
/// Stable across application restarts — safe for distributed cache keys (e.g. Redis).
/// </summary>
public static class HashCodeBuilder
{
    /// <summary>
    /// Builds a deterministic, truncated SHA256 hex string from the given values.
    /// Order of values matters.
    /// </summary>
    public static string Build(params object?[] values)
    {
        var sb = new StringBuilder();

        foreach (var value in values)
            sb.Append(value?.ToString() ?? "null").Append('|');

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString()));
        return Convert.ToHexString(bytes)[..16].ToLower();
    }
}