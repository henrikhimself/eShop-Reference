using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.EntityFramework.Options;
using Duende.IdentityServer.Models;
using Hj.IdentityServer;
using Hj.IdentityServer.CustomIdentityResource;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hj.Migration;

internal sealed class IdentityServerResource
{
  private readonly SqlServerHelper _sqlServerHelper;
  private readonly IdentityServerHelper _identityServerHelper;
  private readonly PasswordHasher<IdentityUser> _passwordHasher;

  public IdentityServerResource(
    SqlServerHelper sqlServerHelper,
    IdentityServerHelper identityServerHelper,
    PasswordHasher<IdentityUser> passwordHasher)
  {
    _sqlServerHelper = sqlServerHelper;
    _identityServerHelper = identityServerHelper;
    _passwordHasher = passwordHasher;
  }

  public void Execute()
  {
    EnsureConfigurationDb();
    EnsurePersistedGrantDb();
    EnsureIdentityDb();
  }

  private static void AddRoles(AspNetIdentityDbContext dbContext)
  {
    dbContext.AddRole(Authentication.RoleWebEditors);
    dbContext.AddRole(Authentication.RoleWebAdmins);
    dbContext.AddRole(Authentication.RoleCommerceSettingsAdmins);
    dbContext.AddRole(Authentication.RoleCatalogManagers);
    dbContext.AddRole(Authentication.RoleCommerceAdmins);

    dbContext.SaveChanges();
  }

  private void EnsureConfigurationDb()
  {
    var dbContextOptions
      = _sqlServerHelper.GetDbContextOptions<ConfigurationDbContext>(ServiceName.SqlServer, CommonServiceName.IdentityServerConfigurationDb);
    using var dbContext = new ConfigurationDbContext(dbContextOptions);
    dbContext.StoreOptions = new ConfigurationStoreOptions();
    dbContext.Database.EnsureCreated();

    dbContext.IdentityResources.ExecuteDelete();
    dbContext.ApiScopes.ExecuteDelete();
    dbContext.Clients.ExecuteDelete();

    AddWebClient(dbContext);
  }

  private void EnsurePersistedGrantDb()
  {
    var dbContextOptions
      = _sqlServerHelper.GetDbContextOptions<PersistedGrantDbContext>(ServiceName.SqlServer, CommonServiceName.IdentityServerPersistedGrantDb);
    using var dbContext = new PersistedGrantDbContext(dbContextOptions);
    dbContext.StoreOptions = new OperationalStoreOptions();
    dbContext.Database.EnsureCreated();
  }

  private void EnsureIdentityDb()
  {
    var dbContextOptions
      = _sqlServerHelper.GetDbContextOptions<AspNetIdentityDbContext>(ServiceName.SqlServer, CommonServiceName.IdentityServerIdentityDb);
    using var dbContext = new AspNetIdentityDbContext(dbContextOptions);
    dbContext.Database.EnsureCreated();

    dbContext.Users.ExecuteDelete();
    dbContext.UserClaims.ExecuteDelete();
    dbContext.UserRoles.ExecuteDelete();
    dbContext.Roles.ExecuteDelete();
    dbContext.RoleClaims.ExecuteDelete();

    AddRoles(dbContext);
    AddUsers(dbContext);
  }

  private void AddWebClient(ConfigurationDbContext dbContext)
  {
    // Add OpenID Connect web client for the following service names:
    string[] serviceNames = [
      CommonServiceName.BasketApi,
      CommonServiceName.ProfileApi,
      ServiceName.CommerceWeb,
      ServiceName.Shop1Web];

    var client = _identityServerHelper.CreateOpenIdConnectClient(Authentication.WebClientId, [Authentication.WebClientSecret], serviceNames, options =>
    {
      options.AllowOfflineAccess = true;
      options.AllowedScopes = Authentication.WebClientScopes;
      options.AlwaysIncludeUserClaimsInIdToken = true;
    });
    dbContext.Clients.Add(client.ToEntity());

    dbContext.IdentityResources.Add(new IdentityResources.OpenId().ToEntity());
    dbContext.IdentityResources.Add(new IdentityResources.Email().ToEntity());
    dbContext.IdentityResources.Add(new Role().ToEntity());

    dbContext.ApiScopes.Add(new ApiScope(CommonServiceName.BasketApi).ToEntity());
    dbContext.ApiScopes.Add(new ApiScope(CommonServiceName.ProfileApi).ToEntity());

    dbContext.SaveChanges();
  }

  private void AddUsers(AspNetIdentityDbContext dbContext)
  {
    // Shop user
    dbContext.AddUser(user =>
    {
      user.UserName = "bob";
      user.PasswordHash = _passwordHasher.HashPassword(user, "bob");
      user.Email = "bob@localhost";
      user.EmailConfirmed = true;
    });

    // Optimizely CMS admin
    dbContext.AddUser(
      user =>
      {
        user.UserName = "admin";
        user.PasswordHash = _passwordHasher.HashPassword(user, "admin");
        user.Email = "admin@localhost";
        user.EmailConfirmed = true;
      },
      [Authentication.RoleWebAdmins]);

    // Optimizely CMS editor
    dbContext.AddUser(
      user =>
      {
        user.UserName = "editor";
        user.PasswordHash = _passwordHasher.HashPassword(user, "editor");
        user.Email = "editor@localhost";
        user.EmailConfirmed = true;
      },
      [Authentication.RoleWebEditors]);

    _ = dbContext.SaveChanges();
  }
}
