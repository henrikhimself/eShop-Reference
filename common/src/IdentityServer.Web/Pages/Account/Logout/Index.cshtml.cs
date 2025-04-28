using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Hj.IdentityServer.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hj.IdentityServer.Pages.Account.Logout;

[SecurityHeaders]
[AllowAnonymous]
internal sealed class Index : PageModel
{
  private readonly IIdentityServerInteractionService _interaction;
  private readonly IEventService _events;
  private readonly SignInManager<IdentityUser> _signInManager;

  public Index(
    IIdentityServerInteractionService interaction,
    IEventService events,
    SignInManager<IdentityUser> signInManager)
  {
    _interaction = interaction;
    _events = events;
    _signInManager = signInManager;
  }

  [BindProperty]
  public string? LogoutId { get; set; }

  public async Task<IActionResult> OnGetAsync(string? logoutId)
  {
    LogoutId = logoutId;
    return await OnPostAsync();
  }

  public async Task<IActionResult> OnPostAsync()
  {
    if (User.Identity?.IsAuthenticated == true)
    {
      LogoutId ??= await _interaction.CreateLogoutContextAsync();
      await _signInManager.SignOutAsync();
      await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
    }

    return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
  }
}
