using Hj.Profile.Dto;

namespace Hj.Profile.Repositories;

internal interface IProfileRepository
{
  Task<ProfileDto> GetProfileAsync(HttpContext httpContext);
}
