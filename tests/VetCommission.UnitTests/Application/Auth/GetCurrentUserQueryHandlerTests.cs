using FluentAssertions;
using VetCommission.Application.Common.Auth;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Features.Auth;
using VetCommission.Application.Features.Auth.Me;

namespace VetCommission.UnitTests.Application.Auth;

public sealed class GetCurrentUserQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCurrentSession_WhenAuthenticatedUserExists()
    {
        var user = CreateUser();
        var handler = new GetCurrentUserQueryHandler(
            new StubCurrentUser(user.Id),
            new StubAuthRepository(user));

        var result = await handler.Handle(new GetCurrentUserQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Usuario.Id.Should().Be(user.Id);
        result.Value.Tenants.Should().ContainSingle();
        result.Value.Tenants[0].Recursos.Should().Contain("app.access");
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        var handler = new GetCurrentUserQueryHandler(
            new StubCurrentUser(null),
            new StubAuthRepository(CreateUser()));

        var result = await handler.Handle(new GetCurrentUserQuery(), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Unauthorized);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserDoesNotExist()
    {
        var handler = new GetCurrentUserQueryHandler(
            new StubCurrentUser(Guid.NewGuid()),
            new StubAuthRepository(null));

        var result = await handler.Handle(new GetCurrentUserQuery(), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Unauthorized);
    }

    private static AuthUserRecord CreateUser()
    {
        return new AuthUserRecord(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Administrador Inicial",
            "admin@vetcommission.local",
            "ADMIN@VETCOMMISSION.LOCAL",
            "hash",
            true,
            [
                new AuthTenantRecord(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Tenant Inicial",
                    Guid.Parse("33333333-3333-3333-3333-333333333331"),
                    "Administrador",
                    ["app.access"])
            ]);
    }

    private sealed class StubCurrentUser(Guid? userId) : ICurrentUser
    {
        public bool IsAuthenticated => userId.HasValue;

        public Guid? UserId => userId;
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

        public Task<bool> UserHasAccessAsync(Guid userId, Guid tenantId, string resource, CancellationToken cancellationToken) =>
            Task.FromResult(false);
    }
}
