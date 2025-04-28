using Duende.IdentityServer.Services;
using Hj.IdentityServer.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hj.IdentityServer.Pages.Account.Logout;

[SecurityHeaders]
[AllowAnonymous]
internal sealed class LoggedOut : PageModel
{
  private readonly IIdentityServerInteractionService _interactionService;

  public LoggedOut(IIdentityServerInteractionService interactionService) => _interactionService = interactionService;

  public string? PostLogoutRedirectUri { get; set; }

  public async Task OnGetAsync(string? logoutId)
  {
    var logout = await _interactionService.GetLogoutContextAsync(logoutId);
    PostLogoutRedirectUri = logout?.PostLogoutRedirectUri;
  }
}
