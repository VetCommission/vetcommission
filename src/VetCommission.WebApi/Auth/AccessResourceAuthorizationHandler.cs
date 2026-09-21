using Microsoft.AspNetCore.Authorization;
using VetCommission.Application.Common.Auth;
using VetCommission.Application.Features.Auth;
using VetCommission.Application.Features.Auth.Tenant;

namespace VetCommission.WebApi.Auth;

public sealed class AccessResourceAuthorizationHandler(
    ICurrentUser currentUser,
    ITenantContext tenantContext,
    IAuthRepository authRepository,
    IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<AccessResourceRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AccessResourceRequirement requirement)
    {
        if (currentUser.UserId is not Guid userId || tenantContext.TenantId is not Guid tenantId)
        {
            return;
        }

        if (await authRepository.UserHasAccessAsync(
                userId,
                tenantId,
                requirement.Resource,
                httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None))
        {
            context.Succeed(requirement);
        }
    }
}
