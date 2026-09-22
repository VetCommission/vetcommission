using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using VetCommission.Application.Features.Auth;
using VetCommission.Infrastructure.Auth;
using VetCommission.Infrastructure.Persistence.Generated;
using VetCommission.Infrastructure.Professionals;
using VetCommission.Application.Features.Professionals;
using VetCommission.Application.Features.ProfessionalMasterData;
using VetCommission.Application.Features.ProfessionalRoles;
using VetCommission.Application.Features.ProfessionalSpecialties;

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
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IProfessionalMasterDataRepository, ProfessionalMasterDataRepository>();
            services.AddScoped<IProfessionalRoleRepository, ProfessionalRoleRepository>();
            services.AddScoped<IProfessionalSpecialtyRepository, ProfessionalSpecialtyRepository>();
        }
        else
        {
            services.AddScoped<IAuthRepository, UnavailableAuthRepository>();
            services.AddScoped<IProfessionalRepository, UnavailableProfessionalRepository>();
            services.AddScoped<IProfessionalMasterDataRepository, UnavailableProfessionalMasterDataRepository>();
            services.AddScoped<IProfessionalRoleRepository, UnavailableProfessionalRoleRepository>();
            services.AddScoped<IProfessionalSpecialtyRepository, UnavailableProfessionalSpecialtyRepository>();
        }

        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
        return services;
    }
}
