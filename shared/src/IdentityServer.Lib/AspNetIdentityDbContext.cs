using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hj.IdentityServer;

public sealed class AspNetIdentityDbContext : IdentityDbContext
{
  public AspNetIdentityDbContext(DbContextOptions<AspNetIdentityDbContext> options)
    : base(options)
  {
  }

  public void AddRole(string roleName, ICollection<(string type, string? value)>? claims = null)
  {
    var role = new IdentityRole(roleName);
    role.NormalizedName = role.Name?.ToUpperInvariant();
    role.ConcurrencyStamp = Guid.NewGuid().ToString("D");
    Roles.Add(role);

    if (claims != null)
    {
      foreach (var (type, value) in claims)
      {
        RoleClaims.Add(new()
        {
          RoleId = role.Id,
          ClaimType = type,
          ClaimValue = value,
        });
      }
    }
  }

  public void AddUser(
    Action<IdentityUser> configureUser,
    ICollection<string>? roleNames = null,
    ICollection<(string type, string? value)>? claims = null)
  {
    var user = new IdentityUser();
    configureUser(user);
    user.NormalizedUserName = user.UserName?.ToUpperInvariant();
    user.NormalizedEmail = user.Email?.ToUpperInvariant();
    Users.Add(user);

    if (roleNames != null)
    {
      foreach (var roleName in roleNames)
      {
        var role = Roles.SingleOrDefault(x => x.Name == roleName);
        if (role != null)
        {
          UserRoles.Add(new()
          {
            UserId = user.Id,
            RoleId = role.Id,
          });
        }
      }
    }

    if (claims != null)
    {
      foreach (var (type, value) in claims)
      {
        UserClaims.Add(new IdentityUserClaim<string>()
        {
          UserId = user.Id,
          ClaimType = type,
          ClaimValue = value
        });
      }
    }
  }
}
