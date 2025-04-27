using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hj.IdentityServer.Attributes;

internal sealed class SecurityHeadersAttribute : ActionFilterAttribute
{
  public override void OnResultExecuting(ResultExecutingContext context)
  {
    ArgumentNullException.ThrowIfNull(context);

    var result = context.Result;
    if (result is not PageResult)
    {
      return;
    }

    var headers = context.HttpContext.Response.Headers;

    if (!headers.ContainsKey("X-Content-Type-Options"))
    {
      headers.Append("X-Content-Type-Options", "nosniff");
    }

    if (!headers.ContainsKey("X-Frame-Options"))
    {
      headers.Append("X-Frame-Options", "DENY");
    }

    var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
    if (!headers.ContainsKey("Content-Security-Policy"))
    {
      headers.Append("Content-Security-Policy", csp);
    }
    if (!headers.ContainsKey("X-Content-Security-Policy"))
    {
      headers.Append("X-Content-Security-Policy", csp);
    }

    var referrer_policy = "no-referrer";
    if (!headers.ContainsKey("Referrer-Policy"))
    {
      headers.Append("Referrer-Policy", referrer_policy);
    }
  }
}
