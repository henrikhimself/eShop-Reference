using Hj.ServiceClient.Basket;
using Hj.ServiceClient.IdentityService;
using Hj.ServiceClient.Profile;
using Hj.Shared;
using Hj.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hj.ServiceClient;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddIdentityServiceClient(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddHttpClient(SharedServiceName.IdentityServerWeb);
    services.AddScoped<IIdentityServiceClient, IdentityServiceClient>(sp =>
    {
      var httpClient = CreateHttpClient(sp, SharedServiceName.IdentityServerWeb);

      // Set base address manually since service discovery is not supported.
      var baseAddress = configuration.DiscoverEndpoint($"https://{SharedServiceName.IdentityServerWeb}");
      if (baseAddress != null)
      {
        httpClient.BaseAddress = new Uri(baseAddress);
      }

      return new IdentityServiceClient(httpClient, sp.GetService<IOptionsSnapshot<OpenIdConnectOptions>>());
    });
    return services;
  }

  public static IServiceCollection AddBasketClient(this IServiceCollection services)
  {
    services.AddHttpClient(SharedServiceName.BasketApi, configureClient: client =>
    {
      client.BaseAddress = new($"https+http://{SharedServiceName.BasketApi}");
    });
    services.AddScoped<IBasketClientV1, BasketClientV1>(sp => new BasketClientV1(CreateHttpClient(sp, SharedServiceName.BasketApi, "1")));
    return services;
  }

  public static IServiceCollection AddProfileClient(this IServiceCollection services)
  {
    services.AddHttpClient(SharedServiceName.ProfileApi, configureClient: client =>
    {
      client.BaseAddress = new($"https+http://{SharedServiceName.ProfileApi}");
    });
    services.AddScoped<IProfileClientV1, ProfileClientV1>(sp => new ProfileClientV1(CreateHttpClient(sp, SharedServiceName.ProfileApi, "1")));
    return services;
  }

  private static HttpClient CreateHttpClient(IServiceProvider serviceProvider, string serviceName, string? apiVersion = null)
  {
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(serviceName);
    if (!string.IsNullOrWhiteSpace(apiVersion))
    {
      httpClient.DefaultRequestHeaders.Add(ApiVersioning.ApiVersionHeaderName, apiVersion);
    }
    return httpClient;
  }
}
