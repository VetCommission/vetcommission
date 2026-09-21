namespace VetCommission.Application.Features.Auth;

public sealed record AuthUserRecord(
    Guid Id,
    string Name,
    string Email,
    string NormalizedEmail,
    string PasswordHash,
    bool Active,
    IReadOnlyList<AuthTenantRecord> Tenants);

public sealed record AuthTenantRecord(
    Guid TenantId,
    string Nome,
    Guid GrupoAcessoId,
    string GrupoAcessoNome,
    IReadOnlyList<string> Recursos);

public sealed record JwtTokenResult(string AccessToken, DateTimeOffset ExpiresAt);

public sealed record AuthSessionDto(
    string AccessToken,
    DateTimeOffset ExpiraEm,
    AuthUserDto Usuario,
    IReadOnlyList<AuthTenantDto> Tenants);

public sealed record AuthUserDto(Guid Id, string Nome, string Email);

public sealed record AuthTenantDto(
    Guid TenantId,
    string Nome,
    Guid GrupoAcessoId,
    string GrupoAcessoNome,
    IReadOnlyList<string> Recursos);

public interface IAuthRepository
{
    Task<AuthUserRecord?> FindByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken);

    Task<AuthUserRecord?> FindByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task<bool> UserHasActiveTenantAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken);

    Task<bool> UserHasAccessAsync(
        Guid userId,
        Guid tenantId,
        string resource,
        CancellationToken cancellationToken);
}

public interface IPasswordHasher
{
    bool Verify(string passwordHash, string password);
}

public interface IJwtTokenService
{
    JwtTokenResult CreateToken(AuthUserRecord user);
}
