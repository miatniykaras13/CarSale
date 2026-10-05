namespace ProfileService.Application.Dtos;

public record UpdateMyProfileDto(
    string Username,
    string Email,
    string Name,
    string Surname,
    string PhoneNumber,
    string? Picture = null);
