namespace BuildingBlocks.DDD;

public abstract class Entity<TId> : CSharpFunctionalExtensions.Entity<TId>, IEntity
    where TId : IComparable<TId>
{
    protected Entity()
    {
    }

    protected Entity(TId id)
        : base(id)
    {
    }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}