using Microsoft.EntityFrameworkCore;
using VetCommission.Application.Features.Auth;
using VetCommission.Infrastructure.Persistence.Generated;

namespace VetCommission.Infrastructure.Auth;

public sealed class AuthRepository(VetCommissionDbContext dbContext) : IAuthRepository
{
    public async Task<AuthUserRecord?> FindByNormalizedEmailAsync(
        string normalizedEmail,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.NormalizedEmail == normalizedEmail)
            .Select(user => new AuthUserRecord(
                user.Id,
                user.Name,
                user.Email,
                user.NormalizedEmail,
                user.PasswordHash,
                user.Active,
                user.UserTenants
                    .Where(userTenant =>
                        userTenant.Active &&
                        userTenant.Tenant.Active &&
                        userTenant.AccessGroup.Active)
                    .OrderByDescending(userTenant => userTenant.IsDefault)
                    .ThenBy(userTenant => userTenant.Tenant.Name)
                    .Select(userTenant => new AuthTenantRecord(
                        userTenant.TenantId,
                        userTenant.Tenant.Name,
                        userTenant.AccessGroupId,
                        userTenant.AccessGroup.Name,
                        userTenant.AccessGroup.AccessGroupResources
                            .Where(accessGroupResource => accessGroupResource.AccessResource.Active)
                            .Select(accessGroupResource => accessGroupResource.AccessResource.ResourceKey)
                            .OrderBy(resourceKey => resourceKey)
                            .ToArray()))
                    .ToArray()))
            .SingleOrDefaultAsync(cancellationToken);

        return user;
    }

    public async Task<AuthUserRecord?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new AuthUserRecord(
                user.Id,
                user.Name,
                user.Email,
                user.NormalizedEmail,
                user.PasswordHash,
                user.Active,
                user.UserTenants
                    .Where(userTenant =>
                        userTenant.Active &&
                        userTenant.Tenant.Active &&
                        userTenant.AccessGroup.Active)
                    .OrderByDescending(userTenant => userTenant.IsDefault)
                    .ThenBy(userTenant => userTenant.Tenant.Name)
                    .Select(userTenant => new AuthTenantRecord(
                        userTenant.TenantId,
                        userTenant.Tenant.Name,
                        userTenant.AccessGroupId,
                        userTenant.AccessGroup.Name,
                        userTenant.AccessGroup.AccessGroupResources
                            .Where(accessGroupResource => accessGroupResource.AccessResource.Active)
                            .Select(accessGroupResource => accessGroupResource.AccessResource.ResourceKey)
                            .OrderBy(resourceKey => resourceKey)
                            .ToArray()))
                    .ToArray()))
            .SingleOrDefaultAsync(cancellationToken);

        return user;
    }

    public Task<bool> UserHasActiveTenantAsync(
        Guid userId,
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        return dbContext.UserTenants
            .AsNoTracking()
            .AnyAsync(userTenant =>
                    userTenant.UserId == userId &&
                    userTenant.TenantId == tenantId &&
                    userTenant.Active &&
                    userTenant.User.Active &&
                    userTenant.Tenant.Active &&
                    userTenant.AccessGroup.Active,
                cancellationToken);
    }
}
