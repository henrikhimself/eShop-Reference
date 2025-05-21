using Yarp.ReverseProxy.Configuration;

namespace Hj.ReverseProxy;

internal static class ReverseProxyConfig
{
  public static IEnumerable<RouteConfig> GetRoutes()
  {
    yield return CreateRouteConfig(ServiceName.Shop1Web);
  }

  public static IEnumerable<ClusterConfig> GetClusters(IConfiguration configuration)
  {
    yield return CreateClusterConfig(ServiceName.Shop1Web, configuration);
  }

  private static RouteConfig CreateRouteConfig(string serviceName)
  {
    var routeConfig = new RouteConfig()
    {
      RouteId = serviceName,
      ClusterId = serviceName,
      Match = new RouteMatch()
      {
        Hosts = [serviceName],
      },
    };
    return routeConfig;
  }

  private static ClusterConfig CreateClusterConfig(string serviceName, IConfiguration configuration)
  {
    var endpoints = configuration.DiscoverEndpointList($"http://{ServiceName.Shop1Web}");

    var destinations = endpoints.ToDictionary(k => k, v => new DestinationConfig()
    {
      Address = v,
    });

    var clusterConfig = new ClusterConfig()
    {
      ClusterId = serviceName,
      Destinations = destinations,
    };
    return clusterConfig;
  }
}
