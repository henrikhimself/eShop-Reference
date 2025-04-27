using Hj.Shared.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Hj.ServiceClient.IdentityService;

public interface IIdentityServiceClient
{
  Task<IdentityCredential?> GetClientCredentialAsync(string scope);
  Task<IdentityCredential> GetClientCredentialAsync(string clientId, string? clientSecret, string scope);

  Task RefreshIfExpiredAsync(CookieValidatePrincipalContext context);
  Task<bool> RefreshIfExpiredAsync(IdentityCredential credential);
  Task RefreshAsync(IdentityCredential credential);
}
