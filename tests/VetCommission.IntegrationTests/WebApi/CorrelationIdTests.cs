using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace VetCommission.IntegrationTests.WebApi;

[Collection(WebApiCollection.Name)]
public sealed class CorrelationIdTests
{
    private readonly HttpClient _client;

    public CorrelationIdTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RequestWithCorrelationId_ShouldEchoHeaderInResponse()
    {
        const string correlationId = "vetcommission-test-correlation";
        using var request = new HttpRequestMessage(HttpMethod.Get, "/health");
        request.Headers.Add("X-Correlation-Id", correlationId);

        var response = await _client.SendAsync(request);

        response.Headers.GetValues("X-Correlation-Id").Should().ContainSingle(correlationId);
    }

    [Fact]
    public async Task RequestWithoutCorrelationId_ShouldGenerateResponseHeader()
    {
        var response = await _client.GetAsync("/health");

        response.Headers.TryGetValues("X-Correlation-Id", out var values).Should().BeTrue();
        values.Should().ContainSingle(value => !string.IsNullOrWhiteSpace(value));
    }
}
