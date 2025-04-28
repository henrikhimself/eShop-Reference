using System.Security.Claims;
using Azure.Messaging.ServiceBus;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Hj.Commerce.Features.Messaging;
using Hj.Commerce.Features.Profile;
using Hj.Commerce.Infrastructure.Authentication;
using Hj.DataProtection;
using Hj.MessagingAzure;
using Hj.ServiceClient;
using Mediachase.Commerce.Anonymous;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Hj.Commerce;

internal class Startup
{
  private readonly IWebHostEnvironment _webHostingEnvironment;
  private readonly IConfiguration _configuration;

  public Startup(
    IWebHostEnvironment webHostingEnvironment,
    IConfiguration configuration)
  {
    _webHostingEnvironment = webHostingEnvironment;
    _configuration = configuration;
  }

  public static void Configure(IApplicationBuilder app)
  {
    app.MapDefaultEndpoints();

    app.UseRequestTimeouts();
    app.UseOutputCache();

    app.UseAnonymousId();

    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
      endpoints.MapContent();
    });
  }

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddServiceDefaults(_configuration, _webHostingEnvironment);

    services.Configure<SchedulerOptions>(options => options.Enabled = false);

    services
      .AddIdentityServiceClient(_configuration)
      .AddProfileClient();

    services
      .AddSingleton<EmulatorEventProvider>()
      .AddEventProvider<EmulatorEventProvider>()
      .AddSingleton<IAuthenticationService, AuthenticationService>()
      .AddSingleton<IProfileService, ProfileService>();

    services.ConfigureDataProtection(_configuration, _webHostingEnvironment);

    services
      .AddAzureMessaging(_webHostingEnvironment, configureClients =>
      {
        configureClients
          .AddServiceBusClient(_configuration.GetConnectionString(ServiceName.AzureServiceBus))
          .ConfigureOptions(options =>
          {
            options.TransportType = ServiceBusTransportType.AmqpTcp;
          });
      });

    services.ConfigureOpenIdConnect(
      _configuration,
      cookieOptions: options =>
      {
        options.Events.OnSignedIn = async context => await GetAuthenticationService(context.HttpContext)
          .OnCookiesSignedInAsync(context);

        options.Events.OnValidatePrincipal = async context => await GetAuthenticationService(context.HttpContext)
          .OnValidatePrincipalAsync(context);
      },
      oidcOptions: options =>
      {
        options.ClientId = Authentication.WebClientId;
        options.ClientSecret = Authentication.WebClientSecret;
        options.ResponseType = OpenIdConnectResponseType.Code;

        options.Scope.Clear();
        foreach (var scope in Authentication.WebClientScopes)
        {
          options.Scope.Add(scope);
        }

        options.MapInboundClaims = true;
        options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;
        options.SaveTokens = true;
        options.Events.OnTokenValidated = async context => await GetAuthenticationService(context.HttpContext)
            .OnOidcTokenValidatedAsync(context);
      });

    services
      .AddCommerce()
      .AddEmbeddedLocalization<Startup>()
      .AddAzureBlobProvider(config =>
      {
        config.ConnectionString = _configuration.GetConnectionString(ServiceName.AzureStorageBlobs);
        config.ContainerName = ServiceName.AzureStorageBlobs;
      });
  }

  private static IAuthenticationService GetAuthenticationService(HttpContext httpContext)
    => httpContext.RequestServices.GetRequiredService<IAuthenticationService>();
}
