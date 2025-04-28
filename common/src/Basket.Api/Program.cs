using System.Security.Claims;
using Hj.Basket;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;
var services = builder.Services;

services.AddSharedServiceDefaults(configuration, environment);

services.AddProblemDetails();

services.AddOpenApi();

services
  .AddAuthentication()
  .AddJwtBearer(options =>
  {
    options.Authority = configuration.DiscoverEndpoint($"https://{CommonServiceName.IdentityServerWeb}");
    options.MapInboundClaims = true;
    options.SaveToken = true;
    options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;
    options.TokenValidationParameters.ValidateAudience = false;
  });
services.AddAuthorizationBuilder()
  .AddPolicy("ApiScope", policy =>
  {
    policy.RequireAuthenticatedUser();
    policy.RequireClaim("scope", CommonAuthentication.BasketApiScope);
  });

var app = builder.Build();

app.MapSharedDefaultEndpoints();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
  options.RoutePrefix = string.Empty;
  options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.MapBasketApi();

await app.RunAsync();
