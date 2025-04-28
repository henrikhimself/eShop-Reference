using Hj.ServiceClient.Basket;
using Hj.ServiceClient.IdentityService;
using Hj.ServiceClient.Profile;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Hj.ServiceClient;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddIdentityServiceClient(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddHttpClient(CommonServiceName.IdentityServerWeb);
    services.AddScoped<IIdentityServiceClient, IdentityServiceClient>(sp =>
    {
      var httpClient = CreateHttpClient(sp, CommonServiceName.IdentityServerWeb);

      // Set base address manually since service discovery is not supported.
      var baseAddress = configuration.DiscoverEndpoint($"https://{CommonServiceName.IdentityServerWeb}");
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
    services.AddHttpClient(CommonServiceName.BasketApi, configureClient: client =>
    {
      client.BaseAddress = new($"https+http://{CommonServiceName.BasketApi}");
    });
    services.AddScoped<IBasketClientV1, BasketClientV1>(sp => new BasketClientV1(CreateHttpClient(sp, CommonServiceName.BasketApi, "1")));
    return services;
  }

  public static IServiceCollection AddProfileClient(this IServiceCollection services)
  {
    services.AddHttpClient(CommonServiceName.ProfileApi, configureClient: client =>
    {
      client.BaseAddress = new($"https+http://{CommonServiceName.ProfileApi}");
    });
    services.AddScoped<IProfileClientV1, ProfileClientV1>(sp => new ProfileClientV1(CreateHttpClient(sp, CommonServiceName.ProfileApi, "1")));
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
