using Hj.Shared.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hj.Shared.Extensions;

public static class ApplicationBuilderExtensions
{
  public static bool IsBuild(this IApplicationBuilder builder)
    => BuildHelper.IsBuild(builder.ApplicationServices.GetService<IHostEnvironment>());
}
