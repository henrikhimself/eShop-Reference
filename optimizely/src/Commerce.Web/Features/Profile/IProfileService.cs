using System.Security.Claims;
using Hj.ServiceClient.Profile;

namespace Hj.Commerce.Features.Profile;

public interface IProfileService
{
  Task<ProfileOutputDto?> GetProfileAsync(ClaimsPrincipal? claimsPrincipal);
}
