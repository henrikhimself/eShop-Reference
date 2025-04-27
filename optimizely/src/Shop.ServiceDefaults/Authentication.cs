using Hj.Shared;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Hj.Shop;

public static class Authentication
{
  // OpenID Connect client
  public const string WebClientId = "web";
  public const string WebClientSecret = "73C45B22-1850-6729-8AF4-37ADE40C52B7";
  public static readonly string[] WebClientScopes = [
    OpenIdConnectScope.OfflineAccess,
    OpenIdConnectScope.OpenId,
    OpenIdConnectScope.Email,
    SharedAuthentication.IdentityResource_Role,
    SharedAuthentication.BasketApiScope,
    SharedAuthentication.ProfileApiScope];

  // Optimizely roles
  public const string Role_WebEditors = "WebEditors";
  public const string Role_WebAdmins = "WebAdmins";
  public const string Role_CommerceSettingsAdmins = "CommerceSettingsAdmins";
  public const string Role_CatalogManagers = "CatalogManagers";
  public const string Role_CommerceAdmins = "CommerceAdmins";
}
