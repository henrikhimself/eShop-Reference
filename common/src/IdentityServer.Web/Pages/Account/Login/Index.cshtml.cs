using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Hj.IdentityServer.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hj.IdentityServer.Pages.Account.Login;

[SecurityHeaders]
[AllowAnonymous]
internal sealed class Index : PageModel
{
  private readonly IIdentityServerInteractionService _interaction;
  private readonly IEventService _events;
  private readonly UserManager<IdentityUser> _userManager;
  private readonly SignInManager<IdentityUser> _signInManager;

  public Index(
      IIdentityServerInteractionService interaction,
      IEventService events,
      UserManager<IdentityUser> userManager,
      SignInManager<IdentityUser> signInManager)
  {
    _interaction = interaction;
    _events = events;
    _userManager = userManager;
    _signInManager = signInManager;
  }

  [BindProperty]
  public InputModel? Input { get; set; }

  public void OnGet(string? returnUrl)
  {
    Input = new InputModel
    {
      ReturnUrl = returnUrl,
    };
  }

  public async Task<IActionResult> OnPostAsync()
  {
    if (Input == null)
    {
      return Redirect("~/");
    }

    var returnUrl = Input.ReturnUrl ?? "~/";
    var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

    if (Input.Button != "login")
    {
      if (context != null)
      {
        await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
        return Redirect(returnUrl);
      }

      return Redirect("~/");
    }

    if (ModelState.IsValid)
    {
      var result = await _signInManager.PasswordSignInAsync(Input.Username!, Input.Password!, false, lockoutOnFailure: true);
      if (result.Succeeded)
      {
        var user = await _userManager.FindByNameAsync(Input.Username!);
        await _events.RaiseAsync(new UserLoginSuccessEvent(user!.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

        if (context != null)
        {
          return Redirect(returnUrl);
        }

        if (Url.IsLocalUrl(returnUrl))
        {
          return Redirect(returnUrl);
        }

        throw new ArgumentException("Invalid return URL");
      }

      await _events.RaiseAsync(new UserLoginFailureEvent(Input.Username, "Invalid credentials", clientId: context?.Client.ClientId));
      ModelState.AddModelError(string.Empty, "Invalid username or password");
    }

    Input = new InputModel
    {
      ReturnUrl = returnUrl,
    };
    return Page();
  }
}
