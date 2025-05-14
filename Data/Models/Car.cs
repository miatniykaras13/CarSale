using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Car
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Brand { get; set; } = String.Empty;
    public int Year { get; set; }

    
    public Guid UserId { get; set; }
    
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
}