using System.Security.Claims;
using Hj.ServiceClient.Profile;
using Hj.Shared.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hj.Shop1.Pages;

[Authorize]
public class ProfileModel : PageModel
{
  private readonly IProfileClientV1 _profileClientV1;

  public ProfileModel(IProfileClientV1 profileClientV1) => _profileClientV1 = profileClientV1;

  public List<string>? Identity { get; set; }

  public ProfileOutputDto? Profile { get; set; }

  public async Task<IActionResult> OnGetAsync()
  {
    // Get from local
    var principal = HttpContext.User;
    if (principal.Identities?.Any() ?? false)
    {
      static string FormatIdentity(ClaimsIdentity x)
          => $"AuthType: {x.AuthenticationType}, Name: {x.Name}, NameClaimType: {x.NameClaimType}, Label: {x.Label}";

      foreach (var identity in principal.Identities)
      {
        Identity = [FormatIdentity(identity)];
        if (identity.Actor != null)
        {
          Identity.Add(FormatIdentity(identity.Actor));
        }
      }
    }

    // Get using profile service.
    _profileClientV1.Credential = User.GetCredential();
    Profile = (await _profileClientV1.ProfileAsync()).Profile;

    return Page();
  }
}
