using Hj.Common.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Hj.Common.Extensions;

public static class ApplicationBuilderExtensions
{
  public static bool IsBuild(this IApplicationBuilder builder)
    => BuildHelper.IsBuild(builder.ApplicationServices.GetService<IHostEnvironment>());
}
