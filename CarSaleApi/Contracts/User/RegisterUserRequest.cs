using System.ComponentModel.DataAnnotations;

namespace CarSaleApi.Contracts.User
{
    public record RegisterUserRequest(
        [Required] string Login,
        [Required] string Email,
        [Required] string Password
        );
}
