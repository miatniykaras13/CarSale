using System.ComponentModel.DataAnnotations;

namespace CarSaleApi.Requests
{
    public class RegisterRequest
    {
        
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
