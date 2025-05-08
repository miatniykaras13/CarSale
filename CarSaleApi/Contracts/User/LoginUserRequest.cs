using System.ComponentModel.DataAnnotations;

namespace CarSaleApi.Contracts.User
{
    public record LoginUserRequest(
        [Required] string Email,
        [Required] string Password
        );
}
