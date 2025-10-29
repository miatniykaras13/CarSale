using System.ComponentModel;

namespace BuildingBlocks.Application.Sorting;

public class SortParameters
{
    public string? OrderBy { get; set; }

    [DefaultValue(SortDirection.ASCENDING)]
    public SortDirection? Direction { get; set; }
}