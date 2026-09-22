using System.Net;
using FluentAssertions;

namespace VetCommission.IntegrationTests.WebApi;

[Collection(WebApiCollection.Name)]
public sealed class HealthEndpointTests
{
    private readonly HttpClient _client;

    public HealthEndpointTests(WebApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
