using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using ProfileService.Application.Abstractions.IdentityProvider;
using ProfileService.Application.Dtos;

namespace ProfileService.Infrastructure.Keycloak.Services;

public class AdminKeycloakService(HttpClient keycloakClient, IOptions<KeycloakOptions> options) : IAdminKeycloakService
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public async Task DeleteUserAsync(Guid keycloakId, CancellationToken cancellationToken = default)
    {
        if (keycloakId == Guid.Empty)
            throw new ArgumentException("Keycloak ID must be a non-empty GUID.", nameof(keycloakId));

        using var request = new HttpRequestMessage(HttpMethod.Delete, GetUserPath(keycloakId));
        await SendAuthorizedAsync(request, cancellationToken);
    }

    public async Task UpdateUserAsync(
        Guid keycloakId,
        UpdateKeycloakProfileDto profile,
        CancellationToken cancellationToken = default)
    {
        if (keycloakId == Guid.Empty)
            throw new ArgumentException("Keycloak ID must be a non-empty GUID.", nameof(keycloakId));

        ArgumentNullException.ThrowIfNull(profile);

        if (profile is { Name: null, Surname: null, Email: null, Username: null })
            throw new ArgumentException("At least one profile field must be provided.", nameof(profile));

        var user = new
        {
            FirstName = profile.Name,
            LastName = profile.Surname,
            profile.Email,
            profile.Username,
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, GetUserPath(keycloakId));
        request.Content = JsonContent.Create(user, options: _jsonOptions);
        await SendAuthorizedAsync(request, cancellationToken);
    }

    private string GetUserPath(Guid keycloakId) =>
        $"admin/realms/{Uri.EscapeDataString(options.Value.Realm)}/users/{keycloakId}";

    private async Task SendAuthorizedAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await keycloakClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        var settings = options.Value;
        using var credentials = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = settings.AdminClientId,
            ["client_secret"] = settings.AdminClientSecret,
        });

        using var response = await keycloakClient.PostAsync(
            $"realms/{Uri.EscapeDataString(settings.Realm)}/protocol/openid-connect/token",
            credentials,
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(
            cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(token?.AccessToken))
            throw new InvalidOperationException("Keycloak did not return an access token.");

        return token.AccessToken;
    }

    private sealed record AccessTokenResponse([property: JsonPropertyName("access_token")] string AccessToken);
}