using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Hj.IdentityServer;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment, Action<IdentityBuilder>? builder = null)
  {
    if (environment.IsBuild())
    {
      return services;
    }

    services.AddDbContext<AspNetIdentityDbContext>(options =>
    {
      options.UseSqlServer(configuration.GetConnectionString(CommonServiceName.IdentityServerIdentityDb));
    });

    var identityBuilder = services
      .AddIdentity<IdentityUser, IdentityRole>()
      .AddEntityFrameworkStores<AspNetIdentityDbContext>()
      .AddDefaultTokenProviders();

    builder?.Invoke(identityBuilder);

    return services;
  }
}
