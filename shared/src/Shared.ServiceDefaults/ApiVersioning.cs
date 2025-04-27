using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Hj.Shared;

public static class ApiVersioning
{
  public const string ApiVersionHeaderName = "api-version";

  public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
  {
    services.AddApiVersioning(options =>
    {
      options.AssumeDefaultVersionWhenUnspecified = true;
      options.DefaultApiVersion = new ApiVersion(1);
      options.ReportApiVersions = true;
      options.ApiVersionReader = new HeaderApiVersionReader(ApiVersionHeaderName);
    });
    return services;
  }
}
