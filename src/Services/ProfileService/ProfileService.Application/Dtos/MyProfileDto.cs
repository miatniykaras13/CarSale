namespace ProfileService.Application.Dtos;

public record MyProfileDto(
    Guid Id,
    string Username,
    string Email,
    string Name,
    string Surname,
    string PhoneNumber);
