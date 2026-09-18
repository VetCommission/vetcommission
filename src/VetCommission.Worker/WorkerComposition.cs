using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetCommission.Application;
using VetCommission.Infrastructure;

namespace VetCommission.Worker;

public static class WorkerComposition
{
    public static IServiceCollection AddWorkerComposition(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddLogging();
        services.AddApplication();
        services.AddInfrastructure(configuration);

        return services;
    }
}
