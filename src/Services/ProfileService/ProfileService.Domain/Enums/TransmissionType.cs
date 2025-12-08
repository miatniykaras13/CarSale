using System.Text.Json.Serialization;

namespace ProfileService.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransmissionType
{
    /// <summary>
    ///     Механика
    /// </summary>
    MANUAL = 1,

    /// <summary>
    ///     Автомат
    /// </summary>
    AUTOMATIC = 2,

    /// <summary>
    ///     Вариатор
    /// </summary>
    CVT = 3,
}