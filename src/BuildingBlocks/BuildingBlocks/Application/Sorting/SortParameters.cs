namespace BuildingBlocks.Application.Sorting;

public class SortParameters
{
    public string? OrderBy { get; set; }

    public SortDirection Direction { get; set; } = SortDirection.DESCENDING;
}