using Duende.IdentityServer.Services;
using Hj.IdentityServer.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hj.IdentityServer.Pages.Account.Logout;

[SecurityHeaders]
[AllowAnonymous]
public class LoggedOut : PageModel
{
  private readonly IIdentityServerInteractionService _interactionService;

  public LoggedOutViewModel? View { get; set; }

  public LoggedOut(IIdentityServerInteractionService interactionService) => _interactionService = interactionService;

  public async Task OnGetAsync(string? logoutId)
  {
    var logout = await _interactionService.GetLogoutContextAsync(logoutId);
    View = new LoggedOutViewModel
    {
      PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
    };
  }
}
