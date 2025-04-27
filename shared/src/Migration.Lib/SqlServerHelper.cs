using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Hj.Migration;

public sealed class SqlServerHelper
{
  private readonly IConfiguration _configuration;

  public SqlServerHelper(IConfiguration configuration) => _configuration = configuration;

  public DbContextOptions<TContext> GetDbContextOptions<TContext>(string serviceName, string? databaseName, Action<SqlServerDbContextOptionsBuilder>? sqlServerOptions = null)
    where TContext : DbContext
  {
    var dbContextOptions = new DbContextOptionsBuilder<TContext>()
      .UseSqlServer(GetConnectionString(serviceName, databaseName), sqlServerOptions)
      .Options;
    return dbContextOptions;
  }

  public SqlServerUnit Use(string serviceName, string? databaseName = null) => new(GetConnectionString(serviceName, databaseName));

  private string GetConnectionString(string serviceName, string? databaseName)
  {
    var connectionString = _configuration.GetConnectionString(serviceName);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      throw new InvalidOperationException($"Missing connection string '{serviceName}'");
    }

    var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
    if (!string.IsNullOrWhiteSpace(databaseName))
    {
      connectionStringBuilder.InitialCatalog = databaseName;
    }

    return connectionStringBuilder.ConnectionString;
  }
}
