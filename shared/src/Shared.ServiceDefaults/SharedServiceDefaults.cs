using Hj.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hj.Shared;

public static class SharedServiceDefaults
{
  public static IHostApplicationBuilder AddSharedServiceDefaults(this IHostApplicationBuilder builder)
  {
    builder.Services.AddSharedServiceDefaults(builder.Configuration, builder.Environment);
    return builder;
  }

  public static IServiceCollection AddSharedServiceDefaults(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
  {
    services.ConfigureApiVersioning();

    if (hostEnvironment.IsBuild())
    {
      return services;
    }

    services.ConfigureOpenTelemetry(configuration, hostEnvironment.ApplicationName);

    services.AddSharedDefaultHealthChecks();

    services.AddServiceDiscovery();

    services.ConfigureHttpClientDefaults(http =>
    {
      http.AddStandardResilienceHandler();

      http.AddServiceDiscovery();
    });

    return services;
  }
}
