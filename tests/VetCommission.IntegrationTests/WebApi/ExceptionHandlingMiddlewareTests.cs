using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using VetCommission.WebApi.Middlewares;

namespace VetCommission.IntegrationTests.WebApi;

public sealed class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenUnexpectedExceptionOccurs_ShouldReturnSafeEnvelope()
    {
        var middleware = new ExceptionHandlingMiddleware(
            _ => throw new InvalidOperationException("sensitive stack detail"),
            NullLogger<ExceptionHandlingMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        context.Response.ContentType.Should().Be("application/json");

        context.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(context.Response.Body);
        document.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue();
        errors[0].GetProperty("code").GetString().Should().Be("Unexpected");
        var payload = document.RootElement.GetRawText();
        payload.Should().Contain("Unexpected");
        payload.Should().NotContain("sensitive stack detail");
        payload.Should().NotContain("stack");
    }
}
