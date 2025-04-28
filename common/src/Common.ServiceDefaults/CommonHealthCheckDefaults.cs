using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Hj.Common;

public static class CommonHealthCheckDefaults
{
  internal const string HealthChecksPolicy = "HealthChecks";
  internal const string HealthChecksLiveTag = "live";

  public static IHostApplicationBuilder AddSharedDefaultHealthChecks(this IHostApplicationBuilder builder)
  {
    builder.Services.AddSharedDefaultHealthChecks();
    return builder;
  }

  public static IServiceCollection AddSharedDefaultHealthChecks(this IServiceCollection services)
  {
    services
      .AddRequestTimeouts(static timeouts => timeouts.AddPolicy(HealthChecksPolicy, TimeSpan.FromSeconds(5)))
      .AddOutputCache(static caching => caching.AddPolicy(HealthChecksPolicy, build: static policy => policy.Expire(TimeSpan.FromSeconds(10))));

    services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy(), [HealthChecksLiveTag]);

    return services;
  }
}
