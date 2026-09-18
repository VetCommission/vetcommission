using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using VetCommission.Application.Features.Auth;
using VetCommission.Infrastructure.Auth;
using VetCommission.Infrastructure.Persistence.Generated;

namespace VetCommission.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<VetCommissionDbContext>(options =>
                options
                    .UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure())
                    .UseSnakeCaseNamingConvention());
        }

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

        return services;
    }
}
