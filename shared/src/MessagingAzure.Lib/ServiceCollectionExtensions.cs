using Hj.Shared.Extensions;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hj.MessagingAzure;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddAzureMessaging(this IServiceCollection services, IHostEnvironment environment, Action<AzureClientFactoryBuilder> configureClients)
  {
    if (environment.IsBuild())
    {
      return services;
    }

    services.AddAzureClients(configureClients);

    return services;
  }
}
