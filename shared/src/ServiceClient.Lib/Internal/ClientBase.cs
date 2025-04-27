using System.Text;
using Duende.IdentityModel.Client;
using Hj.Shared.Authentication;

namespace Hj.ServiceClient.Internal;

public abstract class ClientBase : IClientBase
{
  private static readonly AsyncLocal<IdentityCredential?> _credential = new();

  protected ClientBase()
  {
  }

  public IdentityCredential? Credential
  {
    get => _credential.Value;
    set => _credential.Value = value;
  }

  public virtual Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder, CancellationToken ct)
  {
    return Task.CompletedTask;
  }

  public virtual Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string ur, CancellationToken ct)
  {
    var credential = Credential;
    if (credential != null)
    {
      if (credential.AccessToken != null)
      {
        request.SetBearerToken(credential.AccessToken);
      }

      if (!string.IsNullOrWhiteSpace(credential.KeyId))
      {
        request.Headers.Add(credential.KeyIdHeader, credential.KeyId);
      }

      if (!string.IsNullOrWhiteSpace(credential.KeySecret))
      {
        request.Headers.Add(credential.KeySecretHeader, credential.KeySecret);
      }
    }

    return Task.CompletedTask;
  }

  public virtual Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken ct)
  {
    return Task.CompletedTask;
  }
}
