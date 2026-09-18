using Microsoft.AspNetCore.Authorization;
using VetCommission.Application.Features.Auth.Access;

namespace VetCommission.WebApi.Auth;

public static class AuthorizationOptionsExtensions
{
    public static void AddVetCommissionResourcePolicies(this AuthorizationOptions options)
    {
        foreach (var resource in AccessResources.All)
        {
            options.AddPolicy(
                AuthorizationPolicyNames.Resource(resource),
                policy => policy
                    .RequireAuthenticatedUser()
                    .AddRequirements(new AccessResourceRequirement(resource)));
        }
    }
}
