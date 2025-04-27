using System.Text.Json.Nodes;

namespace Hj.Shop.Config;

internal sealed class AzureServiceBusConfig
{
  internal static void Configure(JsonNode configJson)
  {
    configJson["UserConfig"]!["Logging"]!["Type"] = "Console";
  }
}
