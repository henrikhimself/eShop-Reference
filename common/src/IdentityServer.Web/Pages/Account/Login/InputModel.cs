using System.ComponentModel.DataAnnotations;

namespace Hj.IdentityServer.Pages.Account.Login;

internal sealed class InputModel
{
  public string? ReturnUrl { get; set; }

  [Required]
  public string? Username { get; set; }

  [Required]
  public string? Password { get; set; }

  public string? Button { get; set; }
}
