using Microsoft.Data.SqlClient;

namespace Hj.Migration;

public sealed class SqlTransactionUnit : IDisposable
{
  private bool _disposedValue;

  public SqlTransactionUnit(SqlTransaction sqlTransaction)
  {
    SqlTransaction = sqlTransaction;
  }

  public SqlTransaction SqlTransaction { get; private set; }

  public void Commit() => SqlTransaction.Commit();

  public void Rollback() => SqlTransaction.Rollback();

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  private void Dispose(bool disposing)
  {
    if (!_disposedValue)
    {
      if (disposing)
      {
        SqlTransaction.Dispose();
        SqlTransaction.Connection.Dispose();
      }

      _disposedValue = true;
    }
  }
}
