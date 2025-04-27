using System.Security.Claims;
using EPiServer.Security;
using Hj.Commerce.Features.Profile;
using Hj.ServiceClient.IdentityService;
using Hj.Shared.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Hj.Commerce.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
  private readonly ISynchronizingUserService _synchronizingUserService;
  private readonly IIdentityServiceClient _identityServiceClient;
  private readonly IProfileService _profileService;

  public AuthenticationService(
    ISynchronizingUserService synchronizingUserService,
    IIdentityServiceClient identityServiceClient,
    IProfileService profileService)
  {
    _synchronizingUserService = synchronizingUserService;
    _identityServiceClient = identityServiceClient;
    _profileService = profileService;
  }

  public async Task OnValidatePrincipalAsync(CookieValidatePrincipalContext context)
    => await _identityServiceClient.RefreshIfExpiredAsync(context);

  public async Task OnCookiesSignedInAsync(CookieSignedInContext context)
    => await _synchronizingUserService.SynchronizeAsync((ClaimsIdentity?)context.Principal?.Identity);

  public async Task OnOidcTokenValidatedAsync(TokenValidatedContext context)
  {
    var principal = context.Principal;
    if (principal == null
      || principal?.Identity is not ClaimsIdentity identity)
    {
      return;
    }

    context.Principal.SetCredential(context.TokenEndpointResponse);

    var profile = await _profileService.GetProfileAsync(principal);
    if (profile != null)
    {
      identity.AddClaim(new Claim(ClaimTypes.Name, $"{profile.FirstName} {profile.SurName}".Trim()));
      identity.AddClaim(new Claim(ClaimTypes.GivenName, profile.FirstName ?? string.Empty));
      identity.AddClaim(new Claim(ClaimTypes.Surname, profile.SurName ?? string.Empty));
    }
  }
}
