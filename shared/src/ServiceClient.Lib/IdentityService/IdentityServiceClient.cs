using Duende.IdentityModel.Client;
using Hj.Shared;
using Hj.Shared.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace Hj.ServiceClient.IdentityService;

public class IdentityServiceClient : IIdentityServiceClient
{
  private readonly HttpClient _httpClient;
  private readonly IOptionsSnapshot<OpenIdConnectOptions>? _oidcOptions;

  public IdentityServiceClient(
    HttpClient httpClient,
    IOptionsSnapshot<OpenIdConnectOptions>? oidcOptions)
  {
    _httpClient = httpClient;
    _oidcOptions = oidcOptions;
  }

  public async Task<IdentityCredential?> GetClientCredentialAsync(string scope)
  {
    var oidcOptions = _oidcOptions?.Get(SharedAuthentication.OpenIdConnectScheme);
    if (oidcOptions == null || oidcOptions.ClientId == null)
    {
      return null;
    }

    var crendential = await GetClientCredentialAsync(oidcOptions.ClientId, oidcOptions.ClientSecret, scope);
    return crendential;
  }

  public async Task<IdentityCredential> GetClientCredentialAsync(string clientId, string? clientSecret, string scope)
  {
    var disco = await _httpClient.GetDiscoveryDocumentAsync();
    if (disco.IsError)
    {
      throw new InvalidOperationException(disco.Error);
    }

    var tokenRequest = new ClientCredentialsTokenRequest
    {
      Address = disco.TokenEndpoint,
      ClientId = clientId,
      ClientSecret = clientSecret,
      Scope = scope,
    };

    var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
    if (tokenResponse.IsError)
    {
      throw new InvalidOperationException($"{tokenResponse.Error}, {tokenResponse.ErrorDescription}");
    }

    var crendential = new IdentityCredential()
    {
      AccessToken = tokenResponse.AccessToken,
    };
    return crendential;
  }

  public async Task RefreshIfExpiredAsync(CookieValidatePrincipalContext context)
  {
    var principal = context.Principal;
    if (principal == null)
    {
      return;
    }

    var credential = principal.GetCredential();
    if (await RefreshIfExpiredAsync(credential))
    {
      principal.SetCredential(credential);

      var authProperties = new AuthenticationProperties
      {
        IsPersistent = context.Properties?.IsPersistent ?? false,
        ExpiresUtc = context.Properties?.ExpiresUtc,
      };
      await context.HttpContext.SignInAsync(principal, authProperties);
    }
  }

  public async Task<bool> RefreshIfExpiredAsync(IdentityCredential credential)
  {
    if (credential.IsAccessTokenExpired())
    {
      await RefreshAsync(credential);
      return true;
    }
    return false;
  }

  public async Task RefreshAsync(IdentityCredential credential)
  {
    if (credential.RefreshToken == null)
    {
      return;
    }

    var oidcOptions = _oidcOptions?.Get(SharedAuthentication.OpenIdConnectScheme);
    if (oidcOptions == null || oidcOptions.ClientId == null)
    {
      return;
    }

    var disco = await _httpClient.GetDiscoveryDocumentAsync();
    if (disco.IsError)
    {
      throw new InvalidOperationException(disco.Error);
    }

    var refreshRequest = new RefreshTokenRequest()
    {
      Address = disco.TokenEndpoint,
      ClientId = oidcOptions.ClientId,
      ClientSecret = oidcOptions.ClientSecret,
      RefreshToken = credential.RefreshToken,
    };

    var refreshResponse = await _httpClient.RequestRefreshTokenAsync(refreshRequest);
    if (refreshResponse.IsError)
    {
      throw new InvalidOperationException($"{refreshResponse.Error}, {refreshResponse.ErrorDescription}");
    }

    if (refreshResponse.Exception != null)
    {
      throw refreshResponse.Exception;
    }

    credential.AccessToken = refreshResponse.AccessToken;
    credential.RefreshToken = refreshResponse.RefreshToken;
  }
}
