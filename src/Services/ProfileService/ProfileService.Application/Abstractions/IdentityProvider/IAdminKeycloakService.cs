using ProfileService.Application.Dtos;

namespace ProfileService.Application.Abstractions.IdentityProvider;

public interface IAdminKeycloakService
{
    Task DeleteUserAsync(Guid keycloakId, CancellationToken cancellationToken = default);

    Task UpdateUserAsync(
        Guid keycloakId,
        UpdateKeycloakProfileDto profile,
        CancellationToken cancellationToken = default);
}