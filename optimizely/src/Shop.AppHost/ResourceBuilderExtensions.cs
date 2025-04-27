using Microsoft.Extensions.Hosting;

namespace Hj.Shop;

internal static class ResourceBuilderExtensions
{
  public static IResourceBuilder<ProjectResource> AddProjectWithDefaults<TProject>(this IDistributedApplicationBuilder builder, [ResourceName] string name)
    where TProject : IProjectMetadata, new()
  {
    var environment = builder.Environment;

    var resourceBuilder = builder
      .AddProject<TProject>(name, options =>
      {
        options.ExcludeLaunchProfile = true;
        options.ExcludeKestrelEndpoints = true;
      })
      .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment.EnvironmentName)
      .WithEnvironment("DOTNET_ENVIRONMENT", environment.EnvironmentName);

    if (environment.IsDevelopment())
    {
      resourceBuilder.WithEnvironment("DetailedErrors", "true");
    }

    return resourceBuilder;
  }

  public static IResourceBuilder<T> AddEndpointWithDefaults<T>(this IResourceBuilder<T> builder, out EndpointReference endpointReference, bool forceSecure = false, int? port = null)
    where T : IResourceWithEndpoints
  {
    endpointReference = forceSecure
      ? builder.WithHttpsEndpoint(port, name: "https").GetEndpoint("https")
      : builder.WithHttpEndpoint(port, name: "http").GetEndpoint("http");
    return builder;
  }
}
