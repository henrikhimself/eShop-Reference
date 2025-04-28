using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Hj.Shop;

public static class Authentication
{
  // Optimizely roles
  public const string RoleWebEditors = "WebEditors";
  public const string RoleWebAdmins = "WebAdmins";
  public const string RoleCommerceSettingsAdmins = "CommerceSettingsAdmins";
  public const string RoleCatalogManagers = "CatalogManagers";
  public const string RoleCommerceAdmins = "CommerceAdmins";

  // OpenID Connect client
  public const string WebClientId = "web";
  public const string WebClientSecret = "73C45B22-1850-6729-8AF4-37ADE40C52B7";
  public static readonly string[] WebClientScopes = [
    OpenIdConnectScope.OfflineAccess,
    OpenIdConnectScope.OpenId,
    OpenIdConnectScope.Email,
    CommonAuthentication.IdentityResourceRole,
    CommonAuthentication.BasketApiScope,
    CommonAuthentication.ProfileApiScope];
}
