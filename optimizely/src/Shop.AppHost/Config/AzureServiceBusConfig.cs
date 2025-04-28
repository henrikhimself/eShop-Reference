using System.Text.Json.Nodes;

namespace Hj.Shop.Config;

internal static class AzureServiceBusConfig
{
  internal static void Configure(JsonNode configJson)
  {
    configJson["UserConfig"]!["Logging"]!["Type"] = "Console";
  }
}
