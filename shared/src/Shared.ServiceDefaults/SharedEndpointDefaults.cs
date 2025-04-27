using Hj.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Hj.Shared;

public static class SharedEndpointDefaults
{
  public static IApplicationBuilder MapSharedDefaultEndpoints(this IApplicationBuilder app)
  {
    if (app.IsBuild())
    {
      return app;
    }

    app.UseDeveloperExceptionPage();

    app.UseHealthChecks("/health");
    app.UseHealthChecks("/alive", new HealthCheckOptions
    {
      Predicate = static r => r.Tags.Contains(SharedHealthCheckDefaults.HealthChecksLiveTag)
    });

    return app;
  }

  public static WebApplication MapSharedDefaultEndpoints(this WebApplication app)
  {
    if (app.IsBuild())
    {
      return app;
    }

    app.UseDeveloperExceptionPage();

    var healthChecks = app.MapGroup("");
    healthChecks
      .CacheOutput(SharedHealthCheckDefaults.HealthChecksPolicy)
      .WithRequestTimeout(SharedHealthCheckDefaults.HealthChecksPolicy);
    healthChecks.MapHealthChecks("/health");
    healthChecks.MapHealthChecks("/alive", new()
    {
      Predicate = static r => r.Tags.Contains(SharedHealthCheckDefaults.HealthChecksLiveTag)
    });

    return app;
  }
}
