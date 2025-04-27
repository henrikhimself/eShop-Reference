using Hj.Profile.Dto;
using Hj.Profile.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hj.Profile;

public static class ProfileApi
{
  public static IEndpointRouteBuilder MapProfileApi(this IEndpointRouteBuilder app)
  {
    var api = app.NewVersionedApi()
      .WithTags("Profile")
      .RequireAuthorization("ApiScope");
    var v1 = api.MapGroup("api").HasApiVersion(1, 0);

    v1.MapGet("/profile", GetProfileAsync)
      .WithName("Profile")
      .WithSummary("Get profile data");

    return app;
  }

  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
  public static async Task<Ok<ProfileDto>> GetProfileAsync(
    IProfileRepository profileRepository,
    HttpContext httpContext)
  {
    var profile = await profileRepository.GetProfileAsync(httpContext);
    return TypedResults.Ok(profile);
  }
}
