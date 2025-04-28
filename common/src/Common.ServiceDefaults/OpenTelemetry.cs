using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Hj.Common;

public static class OpenTelemetry
{
  public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
  {
    builder.Services.ConfigureOpenTelemetry(builder.Configuration, builder.Environment.ApplicationName);
    return builder;
  }

  public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration, string applicationName)
  {
    services.AddOpenTelemetry()
      .WithMetrics(metrics =>
      {
        metrics
          .AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation()
          .AddRuntimeInstrumentation();
      })
      .WithTracing(tracing =>
      {
        tracing
          .AddSource(applicationName)
          .AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation();
      });

    services.AddOpenTelemetryExporters(configuration);

    return services;
  }

  private static IServiceCollection AddOpenTelemetryExporters(this IServiceCollection services, IConfiguration configuration)
  {
    var useOtlpExporter = !string.IsNullOrWhiteSpace(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
    if (useOtlpExporter)
    {
      services.AddOpenTelemetry().UseOtlpExporter();
    }

    return services;
  }
}
