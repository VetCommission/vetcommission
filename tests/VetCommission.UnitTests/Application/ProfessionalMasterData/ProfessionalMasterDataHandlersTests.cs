using FluentAssertions;
using NSubstitute;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.Auth.Tenant;
using VetCommission.Application.Features.ProfessionalRoles;
using VetCommission.Application.Features.ProfessionalSpecialties;

namespace VetCommission.UnitTests.Application.ProfessionalMasterData;

public sealed class ProfessionalMasterDataHandlersTests
{
    [Fact]
    public async Task ListRoles_ShouldUseActiveTenant()
    {
        var tenantId = Guid.NewGuid();
        var repository = Substitute.For<IProfessionalRoleRepository>();
        repository.ListAsync(tenantId, 1, 20, Arg.Any<CancellationToken>()).Returns(new PagedResult<ProfessionalRoleDto>([new ProfessionalRoleDto(Guid.NewGuid(), "Veterinário", true)], 1, 20, 1));
        var result = await new ProfessionalRoleHandlers(repository, new TenantContext(tenantId)).Handle(new ListProfessionalRolesQuery(1, 20), CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task ListSpecialties_ShouldRejectMissingTenant()
    {
        var repository = Substitute.For<IProfessionalSpecialtyRepository>();
        var result = await new ProfessionalSpecialtyHandlers(repository, new TenantContext(null)).Handle(new ListProfessionalSpecialtiesQuery(1, 20), CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Forbidden);
    }

    private sealed class TenantContext(Guid? id) : ITenantContext
    {
        public Guid? TenantId => id;
    }
}
