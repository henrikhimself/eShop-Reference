using System.IdentityModel.Tokens.Jwt;

namespace Hj.Shared.Authentication;

// This model can store identity credentials, which may include an access token alone
// or paired with a refresh token. It can also hold a key ID and secret pair, along
// with custom header names, for shared secret authentication.
public sealed class IdentityCredential
{
  private string? _accessToken;

  public string? AccessToken
  {
    get => _accessToken;
    set
    {
      _accessToken = value;
      AccessTokenExpiration = null;

      if (_accessToken != null)
      {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_accessToken);
        var expiration = jwt.Payload?.Expiration;
        if (expiration != null)
        {
          AccessTokenExpiration = DateTimeOffset.FromUnixTimeSeconds(expiration.Value).UtcDateTime;
        }
      }
    }
  }

  public DateTime? AccessTokenExpiration { get; set; }

  public string? RefreshToken { get; set; }

  public string KeyIdHeader { get; set; } = nameof(KeyId);
  public string? KeyId { get; set; }

  public string KeySecretHeader { get; set; } = nameof(KeySecret);
  public string? KeySecret { get; set; }

  public bool CanRefresh()
    => !string.IsNullOrWhiteSpace(_accessToken) && !string.IsNullOrWhiteSpace(RefreshToken);

  public bool IsAccessTokenExpired()
    => _accessToken != null && (AccessTokenExpiration ?? DateTime.MaxValue.ToUniversalTime()) < DateTime.UtcNow;
}
