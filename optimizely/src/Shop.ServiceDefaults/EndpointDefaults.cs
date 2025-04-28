using Microsoft.AspNetCore.Builder;

namespace Hj.Shop;

public static class EndpointDefaults
{
  public static IApplicationBuilder MapDefaultEndpoints(this IApplicationBuilder app)
    => app.MapSharedDefaultEndpoints();

  public static WebApplication MapDefaultEndpoints(this WebApplication app)
    => app.MapSharedDefaultEndpoints();
}
