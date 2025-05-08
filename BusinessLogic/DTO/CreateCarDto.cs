using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO;

public class CreateCarDto
{
    public string Brand { get; set; } = String.Empty;
    public int Year { get; set; }

    [Required]
    public string OwnerEmail { get; set; } = String.Empty;
}