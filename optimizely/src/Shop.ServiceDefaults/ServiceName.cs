namespace Hj.Shop;

/// <summary>
/// Names of services provided by resources provisioned by either
/// the AppHost or another orchestrator.
/// </summary>
public static class ServiceName
{
  public const string SqlServer = $"{SolutionName}-sqlserver";
  public const string SqlServerVolume = $"{SolutionName}-sqlserver-data";

  public const string AzureStorage = $"{SolutionName}-azurestorage";
  public const string AzureStorageVolume = $"{SolutionName}-azurestorage-data";
  public const string AzureStorageBlobs = $"azurestorage-blobs";

  public const string AzureServiceBus = $"azureservicebus";

  public const string MigrationApp = "migration-app";

  public const string ReverseProxyWeb = "reverse-proxy-web";

  public const string CommerceWeb = "commerce-web";
  public const string CommerceWebCmsDb = $"{CommerceWeb}-db-cms";
  public const string CommerceWebCommerceDb = $"{CommerceWeb}-db-commerce";

  public const string Shop1Web = "shop1-web";

  /// <summary>
  /// Base prefix for namespacing externally visible names used in the solution.
  /// </summary>
  private const string SolutionName = "optimizely-eshop";
}
