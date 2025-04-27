using System.Security.Claims;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Hj.Shared.Authentication;

public static class ClaimsPrincipalExtensions
{
  private const string AccessToken = "app:access_token";
  private const string RefreshToken = "app:refresh_token";

  public static IdentityCredential GetCredential(this ClaimsPrincipal? source, IdentityCredential? credential = null)
  {
    credential ??= new IdentityCredential();
    if (source == null)
    {
      return credential;
    }

    var accessToken = source.FindFirst(AccessToken)?.Value;
    if (accessToken == null)
    {
      return credential;
    }

    credential.AccessToken = accessToken;
    credential.RefreshToken = source.FindFirst(RefreshToken)?.Value;
    return credential;
  }

  public static void SetCredential(this ClaimsPrincipal? source, IdentityCredential? credential)
  {
    if (source == null || credential == null)
    {
      return;
    }

    if (!string.IsNullOrWhiteSpace(credential.AccessToken))
    {
      UpsertClaim(source, AccessToken, credential.AccessToken);
    }
    if (!string.IsNullOrWhiteSpace(credential.RefreshToken))
    {
      UpsertClaim(source, RefreshToken, credential.RefreshToken);
    }
  }

  public static void SetCredential(this ClaimsPrincipal? source, OpenIdConnectMessage? userToken)
  {
    if (source == null || userToken == null)
    {
      return;
    }

    if (!string.IsNullOrWhiteSpace(userToken.AccessToken))
    {
      UpsertClaim(source, AccessToken, userToken.AccessToken);
    }
    if (!string.IsNullOrWhiteSpace(userToken.RefreshToken))
    {
      UpsertClaim(source, RefreshToken, userToken.RefreshToken);
    }
  }

  private static void UpsertClaim(ClaimsPrincipal principal, string type, string value)
  {
    foreach (var identity in principal.Identities)
    {
      var existingClaim = identity.FindFirst(type);
      if (existingClaim != null)
      {
        identity.RemoveClaim(existingClaim);
      }
      identity.AddClaim(new Claim(type, value));
    }
  }
}
