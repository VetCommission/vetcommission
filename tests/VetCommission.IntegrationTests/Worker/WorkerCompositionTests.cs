using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VetCommission.Worker;

namespace VetCommission.IntegrationTests.Worker;

public sealed class WorkerCompositionTests
{
    [Fact]
    public void AddWorkerComposition_ShouldRegisterApplicationWithoutBusinessHostedServices()
    {
        var configuration = new ConfigurationBuilder().Build();
        var services = new ServiceCollection();

        services.AddWorkerComposition(configuration);
        using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IMediator>().Should().NotBeNull();
        provider.GetServices<IHostedService>().Should().BeEmpty();
    }
}
