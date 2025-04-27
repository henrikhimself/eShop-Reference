using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Hj.Shared;

namespace Hj.IdentityServer;

public static class CustomIdentityResource
{
  public class Role : IdentityResource
  {
    public Role()
    {
      Name = SharedAuthentication.IdentityResource_Role;
      DisplayName = "Your user role";
      UserClaims.Add(JwtClaimTypes.Role);
    }
  }
}
