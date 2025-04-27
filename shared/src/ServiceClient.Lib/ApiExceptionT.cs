namespace Hj.ServiceClient;

public class ApiException<TResult> : ApiException
{
  public TResult Result { get; private set; }

  public ApiException(string message, int statusCode, string? response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception? innerException)
      : base(message, statusCode, response, headers, innerException)
  {
    Result = result;
  }
}
