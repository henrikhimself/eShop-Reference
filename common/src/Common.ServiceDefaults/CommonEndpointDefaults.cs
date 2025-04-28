using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Hj.Common;

public static class CommonEndpointDefaults
{
  public static IApplicationBuilder MapSharedDefaultEndpoints(this IApplicationBuilder app)
  {
    if (app.IsBuild())
    {
      return app;
    }

    app.UseDeveloperExceptionPage();

    app.UseHealthChecks("/health");
    app.UseHealthChecks("/alive", CreateHealthCheckOptions());

    return app;
  }

  public static WebApplication MapSharedDefaultEndpoints(this WebApplication app)
  {
    if (app.IsBuild())
    {
      return app;
    }

    app.UseDeveloperExceptionPage();

    var healthChecks = app.MapGroup(string.Empty);
    healthChecks
      .CacheOutput(CommonHealthCheckDefaults.HealthChecksPolicy)
      .WithRequestTimeout(CommonHealthCheckDefaults.HealthChecksPolicy);
    healthChecks.MapHealthChecks("/health");
    healthChecks.MapHealthChecks("/alive", CreateHealthCheckOptions());

    return app;
  }

  private static HealthCheckOptions CreateHealthCheckOptions() => new()
  {
    Predicate = static r => r.Tags.Contains(CommonHealthCheckDefaults.HealthChecksLiveTag),
  };
}
