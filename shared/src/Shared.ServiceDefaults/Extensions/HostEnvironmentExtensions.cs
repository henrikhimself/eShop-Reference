using Hj.Shared.Internal;
using Microsoft.Extensions.Hosting;

namespace Hj.Shared.Extensions;

public static class HostEnvironmentExtensions
{
  public static bool IsBuild(this IHostEnvironment environment)
    => BuildHelper.IsBuild(environment);
}
