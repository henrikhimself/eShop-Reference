using Hj.Common.Authentication;

namespace Hj.ServiceClient.Internal;

public interface IClientBase
{
  IdentityCredential? Credential { get; set; }
}
