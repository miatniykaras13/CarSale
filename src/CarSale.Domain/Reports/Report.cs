using CarSale.Domain.Reports.Enums;

namespace CarSale.Domain.Reports;

public class Report
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required Guid UserId { get; set; }

    public required Guid ReportedEntityId { get; set; }

    public required string ReportedEntityType { get; set; }

    public required string Reason { get; set; }

    public Status Status { get; set; } = Status.OPENED;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public Guid? ResolvedByUserId { get; set; }
}