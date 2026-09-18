using Microsoft.AspNetCore.Authorization;

namespace VetCommission.WebApi.Auth;

public sealed class AccessResourceAuthorizationHandler
    : AuthorizationHandler<AccessResourceRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AccessResourceRequirement requirement)
    {
        var hasResource = context.User.Claims.Any(claim =>
            claim.Type == JwtTokenService.AccessClaimType &&
            string.Equals(claim.Value, requirement.Resource, StringComparison.Ordinal));

        if (hasResource)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
