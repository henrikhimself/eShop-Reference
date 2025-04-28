using Microsoft.Data.SqlClient;

namespace Hj.Migration;

public sealed class SqlServerUnit : IDisposable
{
  private readonly string _connectionString;

  private SqlTransactionUnit? _transactionUnit;
  private bool _disposedValue;

  public SqlServerUnit(string connectionString) => _connectionString = connectionString;

  public SqlTransactionUnit NewTransaction()
  {
#pragma warning disable CA2000 // Dispose objects before losing scope
    var sqlConnection = new SqlConnection(_connectionString);
#pragma warning restore CA2000 // Dispose objects before losing scope

    _transactionUnit = new SqlTransactionUnit(sqlConnection.BeginTransaction());
    return _transactionUnit;
  }

  public async Task<int> CreateAsync(string? databaseName = null)
  {
    if (await ExistsAsync(databaseName))
    {
      return 0;
    }

    databaseName = GetDatabaseNameOrDefault(databaseName);
    return await ExecuteNonQueryAsync($"CREATE DATABASE [{databaseName}]");
  }

  public async Task<int> DropAsync(string? databaseName = null)
  {
    if (!await ExistsAsync(databaseName))
    {
      return 0;
    }

    databaseName = GetDatabaseNameOrDefault(databaseName);
    return await ExecuteNonQueryAsync($"DROP DATABASE [{databaseName}]");
  }

  public async Task<bool> ExistsAsync(string? databaseName = null)
  {
    databaseName = GetDatabaseNameOrDefault(databaseName);
    var dbCount = await ExecuteScalarAsync(
      "SELECT COUNT(*) FROM sys.databases WHERE name = @databaseName",
      x => x.AddWithValue("databaseName", databaseName));
    return !0.Equals(dbCount);
  }

  public async Task<int> ExecuteNonQueryAsync(string sql, Action<SqlParameterCollection>? parametersFn = null)
  {
    using var command = CreateCommand(sql, parametersFn);
    return await ExecuteNonQueryAsync(command);
  }

  public Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
    => ExecuteAsync(sqlCommand, cmd => cmd.ExecuteNonQueryAsync());

  public async Task<object?> ExecuteScalarAsync(string sql, Action<SqlParameterCollection>? parametersFn = null)
  {
    using var command = CreateCommand(sql, parametersFn);
    return await ExecuteScalarAsync(command);
  }

  public Task<object?> ExecuteScalarAsync(SqlCommand sqlCommand)
    => ExecuteAsync(sqlCommand, cmd => cmd.ExecuteScalarAsync());

  public void Dispose()
  {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }

  private static SqlCommand CreateCommand(string sql, Action<SqlParameterCollection>? parametersFn = null)
  {
    var sqlCommand = new SqlCommand(sql);
    parametersFn?.Invoke(sqlCommand.Parameters);
    return sqlCommand;
  }

  private async Task<T> ExecuteAsync<T>(SqlCommand sqlCommand, Func<SqlCommand, Task<T>> sqlCommandFn)
  {
    if (_transactionUnit == null)
    {
      using var sqlConnection = new SqlConnection(_connectionString);
      await sqlConnection.OpenAsync();
      sqlCommand.Connection = sqlConnection;
      return await sqlCommandFn(sqlCommand);
    }

    sqlCommand.Connection = _transactionUnit.SqlTransaction.Connection;
    sqlCommand.Transaction = _transactionUnit.SqlTransaction;
    return await sqlCommandFn(sqlCommand);
  }

  private string GetDatabaseNameOrDefault(string? databaseName = null)
  {
    databaseName ??= new SqlConnectionStringBuilder(_connectionString).InitialCatalog;
    if (string.IsNullOrWhiteSpace(databaseName))
    {
      throw new InvalidOperationException("Operation requires a database name.");
    }

    return databaseName;
  }

  private void Dispose(bool disposing)
  {
    if (!_disposedValue)
    {
      if (disposing)
      {
        _transactionUnit?.Dispose();
        _transactionUnit = null;
      }

      _disposedValue = true;
    }
  }
}
