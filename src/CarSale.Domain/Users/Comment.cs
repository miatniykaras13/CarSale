namespace CarSale.Domain.Users;

public class Comment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public required User User { get; set; }

    public required Guid CommentedEntityId { get; set; }

    public required string CommentedEntityType { get; set; }
}