using Hj.Shop;

namespace Hj.Migration;

internal sealed class CommerceResource
{
  private readonly AzureStorageHelper _azureStorageHelper;
  private readonly SqlServerHelper _sqlServerHelper;

  public CommerceResource(
    AzureStorageHelper azureStorageHelper,
    SqlServerHelper sqlServerHelper)
  {
    _azureStorageHelper = azureStorageHelper;
    _sqlServerHelper = sqlServerHelper;
  }

  public async Task ExecuteAsync()
  {
    using var db = _sqlServerHelper.Use(ServiceName.SqlServer);
    await db.CreateAsync(ServiceName.CommerceWebCmsDb);
    await db.CreateAsync(ServiceName.CommerceWebCommerceDb);

    await _azureStorageHelper.CreateBlobContainerAsync(ServiceName.AzureStorageBlobs);
  }
}
