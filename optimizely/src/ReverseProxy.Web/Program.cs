using Hj.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddReverseProxy()
  .LoadFromMemory(GetRoutes(), GetClusters());

var app = builder.Build();

app.MapGet("/update", () => "Update reverse proxy configuration!");

app.MapReverseProxy();

await app.RunAsync();

RouteConfig[] GetRoutes() => [.. ReverseProxyConfig.GetRoutes()];
ClusterConfig[] GetClusters() => [.. ReverseProxyConfig.GetClusters(builder.Configuration)];
