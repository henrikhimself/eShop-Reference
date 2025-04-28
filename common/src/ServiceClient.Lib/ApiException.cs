using System.Globalization;
using System.Text;

namespace Hj.ServiceClient;

public class ApiException : Exception
{
  public ApiException(string message, int statusCode, string? response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception? innerException)
      : base(ToString(message, statusCode, response), innerException)
  {
    StatusCode = statusCode;
    Response = response;
    Headers = headers;
  }

  public int StatusCode { get; private set; }

  public string? Response { get; private set; }

  public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

  public override string ToString() => $"HTTP Response: \n\n{Response}\n\n{base.ToString()}";

  private static string ToString(string message, int statusCode, string? response)
  {
    var responseLength = response?.Length ?? 0;
    responseLength = responseLength >= 512
      ? 512
      : responseLength;

    var responseString = response == null
      ? "(null)"
      : response[..responseLength];

    var buffer = new StringBuilder()
      .AppendLine(message)
      .AppendLine(CultureInfo.InvariantCulture, $"Status: {statusCode}")
      .AppendLine(CultureInfo.InvariantCulture, $"Response: {responseString}");
    return buffer.ToString();
  }
}
