using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;


    [Required]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;

    [Required]
    public string Password { get; set; } = String.Empty;

    public List<Car> Cars { get; set; } = new();
    
    public DateTime Created { get; set; } = DateTime.UtcNow;
}