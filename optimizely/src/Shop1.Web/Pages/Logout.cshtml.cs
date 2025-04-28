using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hj.Shop1.Pages;

internal sealed class Logout : PageModel
{
  public IActionResult OnGet()
  {
    return SignOut(CommonAuthentication.CookiesScheme, CommonAuthentication.OpenIdConnectScheme);
  }
}
