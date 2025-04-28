using System.Security.Claims;
using Hj.ServiceClient.Profile;

namespace Hj.Commerce.Features.Profile;

internal interface IProfileService
{
  Task<ProfileOutputDto?> GetProfileAsync(ClaimsPrincipal? claimsPrincipal);
}
