using Hj.Shared;
using Hj.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
      options.UseSqlServer(configuration.GetConnectionString(SharedServiceName.IdentityServerIdentityDb));
    });

    var identityBuilder = services
      .AddIdentity<IdentityUser, IdentityRole>()
      .AddEntityFrameworkStores<AspNetIdentityDbContext>()
      .AddDefaultTokenProviders();

    builder?.Invoke(identityBuilder);

    return services;
  }
}
