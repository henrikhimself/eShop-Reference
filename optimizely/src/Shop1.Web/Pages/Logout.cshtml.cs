using Hj.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hj.Shop1.Pages;

public class LogoutModel : PageModel
{
  public IActionResult OnGet()
  {
    return SignOut(SharedAuthentication.CookiesScheme, SharedAuthentication.OpenIdConnectScheme);
  }
}
