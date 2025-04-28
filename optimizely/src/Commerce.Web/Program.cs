using Hj.Commerce;

var builder = Host.CreateDefaultBuilder(args)
  .ConfigureCmsDefaults()
  .ConfigureAppConfiguration((ctx, configBuilder) =>
  {
    configBuilder.AddJsonFile($"appsettings.{Environment.MachineName.ToUpperInvariant()}.json", true, true);
  })
  .ConfigureLogging(builder =>
  {
    builder.AddOpenTelemetry(logging =>
    {
      logging.IncludeFormattedMessage = true;
      logging.IncludeScopes = true;
    });
  })
  .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

var app = builder.Build();
await app.RunAsync();
