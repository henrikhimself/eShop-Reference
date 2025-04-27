using System.Security.Claims;
using Hj.DataProtection;
using Hj.ServiceClient;
using Hj.ServiceClient.IdentityService;
using Hj.Shared;
using Hj.Shared.Authentication;
using Hj.Shop;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;
var services = builder.Services;

services.AddServiceDefaults(configuration, environment);

services.AddRazorPages();

services.ConfigureDataProtection(configuration, environment);

services
  .AddIdentityServiceClient(configuration)
  .AddBasketClient()
  .AddProfileClient();

services.ConfigureOpenIdConnect(
  configuration,
  cookieOptions: options =>
  {
    options.Events.OnValidatePrincipal = async context =>
    {
      var identityServerClient = context.HttpContext.RequestServices.GetRequiredService<IIdentityServiceClient>();
      await identityServerClient.RefreshIfExpiredAsync(context);
    };
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
    options.SignedOutRedirectUri = "/";
    options.AccessDeniedPath = "/";
    options.Events.OnTokenValidated = context =>
    {
      context.Principal.SetCredential(context.TokenEndpointResponse);
      return Task.CompletedTask;
    };
  });

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
