using Hj.Profile.Dto;

namespace Hj.Profile.Repositories;

public interface IProfileRepository
{
  Task<ProfileDto> GetProfileAsync(HttpContext httpContext);
}
