using System.Text;
using Hj.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hj.Shared;

public static class SharedAuthentication
{
  public const string CookiesScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  public const string OpenIdConnectScheme = OpenIdConnectDefaults.AuthenticationScheme;

  // Service scopes
  public const string BasketApiScope = SharedServiceName.BasketApi;
  public const string ProfileApiScope = SharedServiceName.ProfileApi;

  // Custom identity resources
  public const string IdentityResource_Role = "role";

  public static IServiceCollection ConfigureOpenIdConnect(
    this IServiceCollection services,
    IConfiguration configuration,
    string schemeName = CookiesScheme,
    Action<CookieAuthenticationOptions>? cookieOptions = null,
    Action<OpenIdConnectOptions>? oidcOptions = null)
  {
    services
      .AddAuthentication(options =>
      {
        options.DefaultScheme = schemeName;
        options.DefaultChallengeScheme = OpenIdConnectScheme;
      })
      .AddCookie(schemeName, options =>
      {
        cookieOptions?.Invoke(options);
      })
      .AddOpenIdConnect(OpenIdConnectScheme, options =>
      {
        options.Authority = configuration.DiscoverEndpoint($"https://{SharedServiceName.IdentityServerWeb}");
        options.Events.OnRedirectToIdentityProvider = context =>
        {
          context.Response.Headers.CacheControl = "no-cache,no-store";
          if (context.Response.StatusCode == 401)
          {
            context.HandleResponse();
          }
          return Task.CompletedTask;
        };
        options.Events.OnAuthenticationFailed = async context =>
        {
          context.HandleResponse();
          await context.Response.BodyWriter.WriteAsync(Encoding.ASCII.GetBytes(context.Exception.Message) ?? []);
        };
        oidcOptions?.Invoke(options);
      });

    return services;
  }
}
