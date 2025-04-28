using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hj.DataProtection;

public sealed class DataProtectionDbContext : DbContext, IDataProtectionKeyContext
{
  public DataProtectionDbContext(DbContextOptions<DataProtectionDbContext> options)
    : base(options)
  {
  }

  public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
}
