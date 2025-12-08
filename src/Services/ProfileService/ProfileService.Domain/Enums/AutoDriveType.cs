using System.Text.Json.Serialization;

namespace ProfileService.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AutoDriveType
{
    /// <summary>
    ///     Задний привод
    /// </summary>
    RWD = 1,

    /// <summary>
    ///     Передний привод
    /// </summary>
    FWD = 2,

    /// <summary>
    ///     Полный привод
    /// </summary>
    AWD = 3,
}