using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace Hj.Shared.Internal;

internal static class BuildHelper
{
  public static bool IsBuild(IHostEnvironment? hostEnvironment = null)
  {
    if (hostEnvironment?.IsEnvironment("Build") ?? false)
    {
      return true;
    }
    return Assembly.GetEntryAssembly()?.GetName().Name == "GetDocument.Insider";
  }
}
