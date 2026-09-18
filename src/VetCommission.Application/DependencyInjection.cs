using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VetCommission.Application.Common.Behaviors;
using VetCommission.Application.Features.Auth.Tenant;

namespace VetCommission.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddScoped<ITenantContextValidator, TenantContextValidator>();

        return services;
    }
}
