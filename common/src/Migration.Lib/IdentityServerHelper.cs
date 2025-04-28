using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Hj.Migration;

public sealed class IdentityServerHelper
{
  private readonly IConfiguration _configuration;

  public IdentityServerHelper(IConfiguration configuration) => _configuration = configuration;

  public Client CreateOpenIdConnectClient(string id, ICollection<string> secrets, ICollection<string> serviceNames, Action<Client>? configureClient = null)
  {
    var serviceEndpoints = serviceNames
      .SelectMany(x => _configuration.DiscoverEndpointList($"https+http://{x}"))
      .ToArray();

    var client = new Client()
    {
      ClientId = id,
      ClientSecrets = [.. secrets.Select(x => new Secret(x.Sha256()))],

      // Use authorization code flow.
      AllowedGrantTypes = [OpenIdConnectGrantTypes.AuthorizationCode, OpenIdConnectGrantTypes.ClientCredentials],

      // Redirect here after login.
      RedirectUris = [.. serviceEndpoints.Select(x => x + "/signin-oidc")],

      // Redirect here after logout.
      PostLogoutRedirectUris = [.. serviceEndpoints.Select(x => x + "/signout-callback-oidc")],

      // Limit to 15 minutes to reduce exposure if an access token token is stolen.
      AccessTokenLifetime = 900,

      // Use onetime refresh tokens to prevent replay attacks.
      RefreshTokenUsage = TokenUsage.OneTimeOnly,
    };

    configureClient?.Invoke(client);

    return client;
  }
}
