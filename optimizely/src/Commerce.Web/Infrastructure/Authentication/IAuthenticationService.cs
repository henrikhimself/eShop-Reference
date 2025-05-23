using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Hj.Commerce.Infrastructure.Authentication;

internal interface IAuthenticationService
{
  Task OnValidatePrincipalAsync(CookieValidatePrincipalContext context);

  Task OnCookiesSignedInAsync(CookieSignedInContext context);

  Task OnOidcTokenValidatedAsync(TokenValidatedContext context);
}
