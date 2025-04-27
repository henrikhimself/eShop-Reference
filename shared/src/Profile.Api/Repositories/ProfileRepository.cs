using System.Security.Claims;
using Hj.Profile.Dto;
using Microsoft.AspNetCore.Authentication;

namespace Hj.Profile.Repositories;

public class ProfileRepository : IProfileRepository
{
  public async Task<ProfileDto> GetProfileAsync(HttpContext httpContext)
  {
    var profileOutput = new ProfileOutputDto();
    var profile = new ProfileDto()
    {
      Profile = profileOutput,
    };

    profileOutput.FirstName = "Bendy";
    profileOutput.SurName = "Spruce " + Random.Shared.NextInt64();

    var principal = httpContext.User;

    if (principal.Identities?.Any() ?? false)
    {
      foreach (var identity in principal.Identities)
      {
        static string FormatIdentity(ClaimsIdentity x)
          => $"AuthType: {x.AuthenticationType}, Name: {x.Name}, NameClaimType: {x.NameClaimType}, Label: {x.Label}";
        profileOutput.Identity = [FormatIdentity(identity)];
        if (identity.Actor != null)
        {
          profileOutput.Identity.Add(FormatIdentity(identity.Actor));
        }
      }
    }

    profileOutput.Claims = [.. principal.Claims.Select(x => $"{x.Type}: {x.Value}")];

    var authResult = await httpContext.AuthenticateAsync();
    var authItems = authResult?.Properties?.Items;
    if (authItems != null)
    {
      profileOutput.AuthItems = [.. authItems.Select(x => $"{x.Key}: {x.Value}")];
    }

    return profile;
  }
}
