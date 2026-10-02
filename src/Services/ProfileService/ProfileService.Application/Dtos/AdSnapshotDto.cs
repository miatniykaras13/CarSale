namespace ProfileService.Application.Dtos;

public record AdSnapshotDto(
    string AdId,
    string Status,
    string? Title,
    CarSnapshotDto? Car,
    MoneyDto? Price);
