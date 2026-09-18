using FluentAssertions;
using NSubstitute;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Features.Auth;
using VetCommission.Application.Features.Auth.Tenant;

namespace VetCommission.UnitTests.Application.Auth;

public sealed class TenantContextValidatorTests
{
    private readonly IAuthRepository authRepository = Substitute.For<IAuthRepository>();

    [Fact]
    public async Task ValidateAsync_ShouldReturnTenant_WhenUserHasActiveTenant()
    {
        var userId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();
        var validator = new TenantContextValidator(authRepository);

        authRepository
            .UserHasActiveTenantAsync(userId, tenantId, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await validator.ValidateAsync(userId, tenantId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(tenantId);
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnForbidden_WhenUserDoesNotHaveActiveTenant()
    {
        var userId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();
        var validator = new TenantContextValidator(authRepository);

        authRepository
            .UserHasActiveTenantAsync(userId, tenantId, Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await validator.ValidateAsync(userId, tenantId, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Forbidden);
    }
}
