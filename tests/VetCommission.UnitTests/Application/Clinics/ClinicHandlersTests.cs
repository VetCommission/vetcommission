using FluentAssertions;
using NSubstitute;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Features.Auth.Tenant;
using VetCommission.Application.Features.Clinics;

namespace VetCommission.UnitTests.Application.Clinics;

public sealed class ClinicHandlersTests
{
    [Fact]
    public async Task List_ShouldUseActiveTenantAndPagination()
    {
        var tenantId = Guid.NewGuid();
        var repository = Substitute.For<IClinicRepository>();
        repository.ListAsync(tenantId, 1, 20, null, true, Arg.Any<CancellationToken>())
            .Returns(new PagedResult<ClinicDto>([CreateClinic(tenantId)], 1, 20, 1));

        var result = await new ClinicHandlers(repository, new TenantContext(tenantId))
            .Handle(new ListClinicsQuery(1, 20, null, true), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Items.Should().ContainSingle();
        await repository.Received(1).ListAsync(tenantId, 1, 20, null, true, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task List_ShouldRejectMissingTenant()
    {
        var result = await new ClinicHandlers(Substitute.For<IClinicRepository>(), new TenantContext(null))
            .Handle(new ListClinicsQuery(1, 20, null, true), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Forbidden);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFoundWhenClinicDoesNotBelongToTenant()
    {
        var repository = Substitute.For<IClinicRepository>();
        repository.GetAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((ClinicDto?)null);

        var result = await new ClinicHandlers(repository, new TenantContext(Guid.NewGuid()))
            .Handle(new GetClinicQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.NotFound);
    }

    private static ClinicDto CreateClinic(Guid tenantId) => new(Guid.NewGuid(), tenantId, "Clínica Central", null, null, null, null, true, DateTime.UtcNow, null, null);

    private sealed class TenantContext(Guid? id) : ITenantContext
    {
        public Guid? TenantId => id;
    }
}
