using System.Text.Json.Serialization;

namespace BuildingBlocks.Application.Sorting;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    /// <summary>
    /// По возрастанию
    /// </summary>
    ASCENDING = 0,

    /// <summary>
    /// По убыванию
    /// </summary>
    DESCENDING = 1,
}