namespace ProfileService.Infrastructure.Keycloak;

public class KeycloakOptions
{
    public string Endpoint { get; set; } = string.Empty;

    public string Realm { get; set; } = string.Empty;

    public string AdminClientId { get; set; } = string.Empty;

    public string AdminClientSecret { get; set; } = string.Empty;
}