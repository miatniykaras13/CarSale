using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO;

public class CreateCarDto
{
    [Required]
    public string Brand { get; set; } = String.Empty;
    [Required]
    public int Year { get; set; }
}