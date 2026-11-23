namespace ProfileService.Application.Dtos;

public record UpdateKeycloakProfileDto(
    string? Name = null,
    string? Surname = null,
    string? Email = null,
    string? Username = null);
