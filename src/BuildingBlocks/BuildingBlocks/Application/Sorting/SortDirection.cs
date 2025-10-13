using System.Text.Json.Serialization;

namespace BuildingBlocks.Application.Sorting;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    /// <summary>
    /// По убыванию
    /// </summary>
    DESCENDING,

    /// <summary>
    /// По возрастанию
    /// </summary>
    ASCENDING,
}