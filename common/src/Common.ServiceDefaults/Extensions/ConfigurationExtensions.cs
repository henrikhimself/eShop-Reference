using Hj.Common.Internal;
using Microsoft.Extensions.Configuration;

namespace Hj.Common.Extensions;

public static class ConfigurationExtensions
{
  public static string? DiscoverEndpoint(this IConfiguration configuration, string query, string[]? allowedSchemes = null, bool allowAllSchemes = true)
    => ServiceDiscovery.GetServiceEndpoint(configuration, query, allowedSchemes, allowAllSchemes);

  public static string[] DiscoverEndpointList(this IConfiguration configuration, string query, string[]? allowedSchemes = null, bool allowAllSchemes = true)
    => ServiceDiscovery.GetServiceEndpointList(configuration, query, allowedSchemes, allowAllSchemes) ?? [];
}
