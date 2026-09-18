using FluentAssertions;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Features.Auth;
using VetCommission.Application.Features.Auth.Login;

namespace VetCommission.UnitTests.Application.Auth;

public sealed class LoginCommandHandlerTests
{
    [Fact]
    public async Task Handle_returns_session_when_credentials_are_valid()
    {
        var user = new AuthUserRecord(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Administrador Inicial",
            "admin@vetcommission.local",
            "ADMIN@VETCOMMISSION.LOCAL",
            "valid-hash",
            true,
            [
                new AuthTenantRecord(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Tenant Inicial",
                    Guid.Parse("33333333-3333-3333-3333-333333333331"),
                    "Administrador",
                    ["app.access", "admin.dashboard"])
            ]);
        var repository = new StubAuthRepository(user);
        var passwordHasher = new StubPasswordHasher(true);
        var tokenService = new StubJwtTokenService("token", new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var handler = new LoginCommandHandler(repository, passwordHasher, tokenService);

        var result = await handler.Handle(
            new LoginCommand("Admin@VetCommission.Local", "Admin@123"),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.AccessToken.Should().Be("token");
        result.Value.Usuario.Email.Should().Be("admin@vetcommission.local");
        result.Value.Tenants.Should().ContainSingle();
        result.Value.Tenants[0].Recursos.Should().Contain(["app.access", "admin.dashboard"]);
    }

    [Fact]
    public async Task Handle_returns_unauthorized_when_password_is_invalid()
    {
        var user = CreateValidUser();
        var handler = new LoginCommandHandler(
            new StubAuthRepository(user),
            new StubPasswordHasher(false),
            new StubJwtTokenService("token", DateTimeOffset.UtcNow));

        var result = await handler.Handle(new LoginCommand(user.Email, "wrong"), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Unauthorized);
    }

    [Fact]
    public async Task Handle_returns_unauthorized_when_user_is_inactive()
    {
        var user = CreateValidUser(active: false);
        var handler = new LoginCommandHandler(
            new StubAuthRepository(user),
            new StubPasswordHasher(true),
            new StubJwtTokenService("token", DateTimeOffset.UtcNow));

        var result = await handler.Handle(new LoginCommand(user.Email, "Admin@123"), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Unauthorized);
    }

    [Fact]
    public async Task Handle_returns_forbidden_when_user_has_no_active_tenant()
    {
        var user = CreateValidUser(tenants: []);
        var handler = new LoginCommandHandler(
            new StubAuthRepository(user),
            new StubPasswordHasher(true),
            new StubJwtTokenService("token", DateTimeOffset.UtcNow));

        var result = await handler.Handle(new LoginCommand(user.Email, "Admin@123"), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Forbidden);
    }

    private static AuthUserRecord CreateValidUser(bool active = true, IReadOnlyList<AuthTenantRecord>? tenants = null)
    {
        return new AuthUserRecord(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Administrador Inicial",
            "admin@vetcommission.local",
            "ADMIN@VETCOMMISSION.LOCAL",
            "valid-hash",
            active,
            tenants ??
            [
                new AuthTenantRecord(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Tenant Inicial",
                    Guid.Parse("33333333-3333-3333-3333-333333333331"),
                    "Administrador",
                    ["app.access"])
            ]);
    }

    private sealed class StubAuthRepository(AuthUserRecord? user) : IAuthRepository
    {
        public Task<AuthUserRecord?> FindByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult(user);
        }

        public Task<AuthUserRecord?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(user?.Id == userId ? user : null);
        }

        public Task<bool> UserHasActiveTenantAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                user?.Id == userId &&
                user.Tenants.Any(tenant => tenant.TenantId == tenantId));
        }
    }

    private sealed class StubPasswordHasher(bool isValid) : IPasswordHasher
    {
        public bool Verify(string passwordHash, string password)
        {
            return isValid;
        }
    }

    private sealed class StubJwtTokenService(string token, DateTimeOffset expiresAt) : IJwtTokenService
    {
        public JwtTokenResult CreateToken(AuthUserRecord user)
        {
            return new JwtTokenResult(token, expiresAt);
        }
    }
}
