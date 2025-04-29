using Microsoft.Extensions.Configuration;

namespace Hj.Common.Extensions;

public class ConfigurationExtensionsTests
{
  [Fact]
  public void DiscoverEndpoint_GivenMultipleMatching_ReturnsSingleEndpoint()
  {
    // arrange
    var sut = GetSut(new Dictionary<string, string>()
    {
      { "Services:Service:http:0", "http://localhost:80" },
      { "Services:Service:http:1", "http://localhost:81" },
    });

    // act
    var result = sut.DiscoverEndpoint("http://service");

    // assert
    Assert.NotNull(result);
  }

  [Fact]
  public void DiscoverEndpoint_GivenInvalidQuery_ReturnsNull()
  {
    // arrange
    var sut = GetSut();

    // act
    var result = sut.DiscoverEndpoint("invalid-query");

    // assert
    Assert.Null(result);
  }

  [Fact]
  public void DiscoverEndpointList_GivenInvalidQuery_ReturnsEmpty()
  {
    // arrange
    var sut = GetSut();

    // act
    var result = sut.DiscoverEndpointList("invalid-query");

    // assert
    Assert.Empty(result);
  }

  [Fact]
  public void DiscoverEndpointList_GivenMissingService_ReturnsEmpty()
  {
    // arrange
    var sut = GetSut();

    // act
    var result = sut.DiscoverEndpointList("https://");

    // assert
    Assert.Empty(result);
  }

  [Fact]
  public void DiscoverEndpointList_GivenMissingServiceDiscoverySection_ReturnsEmpty()
  {
    // arrange
    var sut = GetSut(new Dictionary<string, string>()
    {
      { "Services:Service:http:0", "http://localhost" },
    });

    // act
    var result = sut.DiscoverEndpointList("https://service");

    // assert
    Assert.Empty(result);
  }

  [Fact]
  public void DiscoverEndpointList_GivenService_ReturnsEndpoints()
  {
    // arrange
    var sut = GetSut(new Dictionary<string, string>()
    {
      { "Services:Service:Endpoint:0", "http://localhost" },
      { "Services:Service:Endpoint:1", "https://localhost" },
    });

    // act
    var result = sut.DiscoverEndpointList("endpoint://service");

    // assert
    Assert.Equal("http://localhost", result[0]);
    Assert.Equal("https://localhost", result[1]);
  }

  [Fact]
  public void DiscoverEndpointList_GivenServiceAndNamedEndpoint_ReturnsNamedEndpoint()
  {
    // arrange
    var sut = GetSut(new Dictionary<string, string>()
    {
      { "Services:Service:Endpoint:0", "https://localhost" },
      { "Services:Service:Named-Endpoint:0", "https://named-endpoint" },
    });

    // act
    var result = sut.DiscoverEndpointList("https://_named-endpoint.service");

    // assert
    var item = Assert.Single(result);
    Assert.Equal("https://named-endpoint", item);
  }

  [Theory]
  [InlineData("http")]
  [InlineData("https")]
  public void DiscoverEndpointList_GivenServiceWithAllowedSchemes_ReturnsEndpointWithAllowedScheme(string allowedScheme)
  {
    // arrange
    var sut = GetSut(new Dictionary<string, string>()
    {
      { "Services:Service:Endpoint:0", "http://localhost" },
      { "Services:Service:Endpoint:1", "https://localhost" },
    });

    string[] allowedSchemes = [allowedScheme];

    // act
    var result = sut.DiscoverEndpointList("http+https://_endpoint.service", allowedSchemes, false);

    // assert
    var item = Assert.Single(result);
    Assert.Equal(allowedScheme + "://localhost", item);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("http+")]
  public void DiscoverEndpointList_GivenInvalidAllowedSchemes_SkipsAllowedSchemeReturnsNone(string allowedScheme)
  {
    // arrange
    var sut = GetSut(new Dictionary<string, string>()
    {
      { "Services:Service:Endpoint:0", "http://localhost" },
      { "Services:Service:Endpoint:1", "https://localhost" },
    });

    string[] allowedSchemes = [allowedScheme];

    // act
    var result = sut.DiscoverEndpointList("http+https://_endpoint.service", allowedSchemes, false);

    // assert
    Assert.Empty(result);
  }

  private static IConfiguration GetSut(Dictionary<string, string> settings = null)
  {
    settings ??= [];
    return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
  }
}
