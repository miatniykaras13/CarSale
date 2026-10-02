namespace ProfileService.Application.Dtos;

public record ProfileByIdDto(
    Guid Id,
    string Username,
    string Email,
    string Name,
    string Surname,
    string PhoneNumber);
