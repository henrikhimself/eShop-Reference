namespace Hj.Migration;

internal sealed class Runner
{
  private readonly DataProtectionResource _dataProtectionResource;
  private readonly IdentityServerResource _identityServerResource;
  private readonly CommerceResource _commerceResource;

  public Runner(
    DataProtectionResource dataProtectionResource,
    IdentityServerResource identityServerResource,
    CommerceResource commerceResource)
  {
    _dataProtectionResource = dataProtectionResource;
    _identityServerResource = identityServerResource;
    _commerceResource = commerceResource;
  }

  public async Task ExecuteAsync()
  {
    _dataProtectionResource.Execute();
    _identityServerResource.Execute();
    await _commerceResource.ExecuteAsync();
  }
}
