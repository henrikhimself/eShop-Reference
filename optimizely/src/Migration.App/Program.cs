using Hj.Migration;
using Microsoft.AspNetCore.Identity;

var builder = Host.CreateDefaultBuilder(args)
  .ConfigureServices((context, services) =>
  {
    services.AddServiceDefaults(context.Configuration, context.HostingEnvironment);

    // Helpers
    services
      .AddSingleton<Runner>()
      .AddSingleton<AzureStorageHelper>()
      .AddSingleton<SqlServerHelper>()
      .AddSingleton<PasswordHasher<IdentityUser>>()
      .AddSingleton<IdentityServerHelper>();

    // Resources
    services
      .AddSingleton<DataProtectionResource>()
      .AddSingleton<IdentityServerResource>()
      .AddSingleton<CommerceResource>();
  });

var host = builder.Build();
await host.Services.GetRequiredService<Runner>().ExecuteAsync();
