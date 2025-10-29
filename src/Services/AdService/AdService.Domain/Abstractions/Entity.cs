namespace AdService.Domain.Abstractions;

public abstract class Entity<TId> : CSharpFunctionalExtensions.Entity<TId>
    where TId : IComparable<TId>
{
    protected Entity(TId id)
        : base(id)
    {
    }

    public DateTime? CreatedAt { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    public string? CreatedBy { get; protected set; }

    public string? UpdatedBy { get; protected set; }
}