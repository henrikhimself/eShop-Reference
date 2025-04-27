using Hj.Shared;
using Hj.Shared.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hj.DataProtection;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection ConfigureDataProtection(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment, Action<IDataProtectionBuilder>? builder = null)
  {
    if (environment.IsBuild())
    {
      return services;
    }

    services.AddDbContext<DataProtectionDbContext>(options =>
    {
      options.UseSqlServer(configuration.GetConnectionString(SharedServiceName.DataProtectionDb));
    });
    var dataProtectionBuilder = services
      .AddDataProtection()
      .SetApplicationName(environment.ApplicationName)
      .PersistKeysToDbContext<DataProtectionDbContext>();

    builder?.Invoke(dataProtectionBuilder);

    return services;
  }
}
