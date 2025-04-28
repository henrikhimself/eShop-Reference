using System.Security.Claims;
using Hj.Common.Authentication;
using Hj.ServiceClient.Profile;

namespace Hj.Commerce.Features.Profile;

internal class ProfileService : IProfileService
{
  private readonly IProfileClientV1 _profileClientV1;

  public ProfileService(IProfileClientV1 profileClientV1)
  {
    _profileClientV1 = profileClientV1;
  }

  public async Task<ProfileOutputDto?> GetProfileAsync(ClaimsPrincipal? claimsPrincipal)
  {
    if (claimsPrincipal == null)
    {
      return null;
    }

    _profileClientV1.Credential = claimsPrincipal.GetCredential();

    var profileDto = await _profileClientV1.ProfileAsync();
    return profileDto.Profile;
  }
}
