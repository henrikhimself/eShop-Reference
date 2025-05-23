using Hj.DataProtection;

namespace Hj.Migration;

internal sealed class DataProtectionResource
{
  private readonly SqlServerHelper _sqlServerHelper;

  public DataProtectionResource(SqlServerHelper sqlServerHelper) => _sqlServerHelper = sqlServerHelper;

  public void Execute()
  {
    var dataProtectionDbContextOptions
      = _sqlServerHelper.GetDbContextOptions<DataProtectionDbContext>(ServiceName.SqlServer, CommonServiceName.DataProtectionDb);
    using var dataProtectionDbContext = new DataProtectionDbContext(dataProtectionDbContextOptions);
    dataProtectionDbContext.Database.EnsureDeleted();
    dataProtectionDbContext.Database.EnsureCreated();
  }
}
