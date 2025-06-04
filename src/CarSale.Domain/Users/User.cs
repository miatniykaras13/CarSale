namespace CarSale.Domain.Users;

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    
    public required string Email { get; set; }
    
    public required string HashedPassword { get; set; }
    
    public string? Login { get; set; }

    public List<Comment> Comments { get; set; } = [];
    
    public List<Role> Roles { get; set; } = [];
    
    public List<Guid> FavoriteAds { get; set; } = [];

    public List<string> SearchHistory { get; set; } = [];
    
    public List<Guid> BrowseHistory { get; set; } = [];
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}