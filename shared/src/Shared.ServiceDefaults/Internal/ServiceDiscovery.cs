using Microsoft.Extensions.Configuration;

namespace Hj.Shared.Internal;

/// <summary>
/// Implements the service discovery algorithm for resolving queries
/// in configuration that do not use a http client.
/// </summary>
internal static class ServiceDiscovery
{
  public static string? GetServiceEndpoint(IConfiguration configuration, string query, string[]? allowedSchemes = null, bool allowAllSchemes = true)
  {
    string? uriString = null;
    var uriStrings = GetServiceEndpointList(configuration, query, allowedSchemes, allowAllSchemes);
    if (uriStrings != null)
    {
      uriString = uriStrings[new Random(Environment.TickCount).Next(uriStrings.Length)];
    }
    return uriString;
  }

  public static string[]? GetServiceEndpointList(IConfiguration configuration, string query, string[]? allowedSchemes = null, bool allowAllSchemes = true)
  {
    if (!Uri.TryCreate(query, UriKind.Absolute, out var uri))
    {
      return null;
    }

    var serviceName = uri.Host;
    var namedEndpoint = string.Empty;
    var namedEndpointSeparator = serviceName.IndexOf('.');
    if (serviceName[0] == '_' && namedEndpointSeparator > 1 && serviceName[^1] != '.')
    {
      namedEndpoint = serviceName[1..namedEndpointSeparator];
      serviceName = serviceName[(namedEndpointSeparator + 1)..];
    }
    allowedSchemes ??= uri.Scheme.Split('+');

    ReadOnlySpan<string> endpoints = [namedEndpoint, .. allowedSchemes];
    foreach (var endpoint in endpoints)
    {
      var section = configuration.GetSection($"Services:{serviceName}:{endpoint}");
      if (section.Exists())
      {
        var uriStrings = section.Get<string[]>();
        if (uriStrings == null || allowAllSchemes)
        {
          return uriStrings;
        }

        uriStrings = [.. uriStrings.Where(x =>
        {
          return Uri.TryCreate(x, default, out var uri)
            && allowedSchemes.Contains(uri.Scheme, StringComparer.OrdinalIgnoreCase);
        })];
        return uriStrings;
      }
    }

    return null;
  }
}
