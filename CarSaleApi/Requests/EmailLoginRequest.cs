using System.ComponentModel.DataAnnotations;

namespace CarSaleApi.Requests
{
    public class EmailLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = String.Empty;

    }
}
