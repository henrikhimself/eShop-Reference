using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace Hj.Migration;

public sealed class AzureStorageHelper
{
  private readonly IConfiguration _configuration;

  public AzureStorageHelper(IConfiguration configuration) => _configuration = configuration;

  public async Task<int> CreateBlobContainerAsync(string serviceName)
  {
    if (await ExistsBlobContainerAsync(serviceName))
    {
      return 0;
    }

    var blobServiceClient = GetBlobServiceClient(serviceName);
    _ = await blobServiceClient.CreateBlobContainerAsync(serviceName, PublicAccessType.BlobContainer);
    return 1;
  }

  public async Task<int> DeleteBlobContainerAsync(string serviceName)
  {
    if (!await ExistsBlobContainerAsync(serviceName))
    {
      return 0;
    }

    var blobServiceClient = GetBlobServiceClient(serviceName);
    _ = await blobServiceClient.DeleteBlobContainerAsync(serviceName);
    return 1;
  }

  public async Task<bool> ExistsBlobContainerAsync(string serviceName)
  {
    var blobServiceClient = GetBlobServiceClient(serviceName);
    var blobContainersEnumerator = blobServiceClient
      .GetBlobContainersAsync()
      .GetAsyncEnumerator();

    while (await blobContainersEnumerator.MoveNextAsync())
    {
      var blobContainersName = blobContainersEnumerator.Current.Name;
      if (string.Equals(blobContainersName, serviceName, StringComparison.OrdinalIgnoreCase))
      {
        return true;
      }
    }

    return false;
  }

  public BlobServiceClient GetBlobServiceClient(string serviceName)
  {
    var connectionString = _configuration.GetConnectionString(serviceName);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      throw new InvalidOperationException($"Missing connection string '{serviceName}'");
    }

    return new BlobServiceClient(connectionString);
  }
}
