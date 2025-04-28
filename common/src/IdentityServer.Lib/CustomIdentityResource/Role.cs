using Duende.IdentityModel;
using Duende.IdentityServer.Models;

namespace Hj.IdentityServer.CustomIdentityResource;

public class Role : IdentityResource
{
  public Role()
  {
    Name = CommonAuthentication.IdentityResourceRole;
    DisplayName = "Your user role";
    UserClaims.Add(JwtClaimTypes.Role);
  }
}
