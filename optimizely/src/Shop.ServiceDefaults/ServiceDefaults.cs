using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Hj.Shop;

public static class ServiceDefaults
{
  public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
  {
    builder.Services.AddServiceDefaults(builder.Configuration, builder.Environment);
    return builder;
  }

  public static IServiceCollection AddServiceDefaults(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    => services.AddSharedServiceDefaults(configuration, environment);
}
