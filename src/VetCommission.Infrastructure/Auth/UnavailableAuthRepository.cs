using VetCommission.Application.Features.Auth;

namespace VetCommission.Infrastructure.Auth;

public sealed class UnavailableAuthRepository : IAuthRepository
{
    public Task<AuthUserRecord?> FindByNormalizedEmailAsync(
        string normalizedEmail,
        CancellationToken cancellationToken)
    {
        throw CreateException();
    }

    public Task<AuthUserRecord?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw CreateException();
    }

    public Task<bool> UserHasActiveTenantAsync(
        Guid userId,
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        throw CreateException();
    }

    public Task<bool> UserHasAccessAsync(
        Guid userId,
        Guid tenantId,
        string resource,
        CancellationToken cancellationToken)
    {
        throw CreateException();
    }

    private static InvalidOperationException CreateException()
    {
        return new InvalidOperationException(
            "ConnectionStrings:DefaultConnection não foi configurada. Configure a conexão com o PostgreSQL para usar autenticação e acesso a dados.");
    }
}
