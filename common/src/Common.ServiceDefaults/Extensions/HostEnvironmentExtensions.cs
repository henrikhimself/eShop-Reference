using Hj.Common.Internal;
using Microsoft.Extensions.Hosting;

namespace Hj.Common.Extensions;

public static class HostEnvironmentExtensions
{
  public static bool IsBuild(this IHostEnvironment environment)
    => BuildHelper.IsBuild(environment);
}
