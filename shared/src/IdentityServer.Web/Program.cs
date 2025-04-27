using Hj.DataProtection;
using Hj.IdentityServer;
using Hj.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;
var services = builder.Services;

services.AddSharedServiceDefaults(configuration, environment);

services.ConfigureDataProtection(configuration, environment);

services.ConfigureIdentity(configuration, environment);

services
  .AddIdentityServer()
  .AddConfigurationStore(options =>
  {
    options.ConfigureDbContext = x => x.UseSqlServer(configuration.GetConnectionString(SharedServiceName.IdentityServerConfigurationDb));
  })
  .AddOperationalStore(options =>
  {
    options.ConfigureDbContext = x => x.UseSqlServer(configuration.GetConnectionString(SharedServiceName.IdentityServerPersistedGrantDb));
  })
  .AddAspNetIdentity<IdentityUser>();

services.AddRazorPages();

var app = builder.Build();

app.MapSharedDefaultEndpoints();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages()
  .RequireAuthorization();

app.Run();
