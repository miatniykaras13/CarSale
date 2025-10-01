using System.Text.Json.Serialization;

namespace AutoCatalog.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FuelType
{
    /// <summary>
    ///     Дизель
    /// </summary>
    DIESEL = 1,

    /// <summary>
    ///     Бензин
    /// </summary>
    PETROL = 2,

    /// <summary>
    ///     Гибрид
    /// </summary>
    HYBRID = 3,

    /// <summary>
    ///     Электро
    /// </summary>
    ELECTRO = 4,
}