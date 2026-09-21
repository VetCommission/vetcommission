using FluentAssertions;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Features.Auth.Tenant;
using VetCommission.Application.Features.Professionals;

namespace VetCommission.UnitTests.Application.Professionals;

public sealed class ProfessionalHandlersTests
{
    [Fact]
    public async Task Create_ShouldUseActiveTenantAndReturnProfessional()
    {
        var tenantId = Guid.NewGuid();
        var repository = new StubProfessionalRepository();
        var handler = new CreateProfessionalHandler(repository, new StubTenantContext(tenantId));
        var command = new CreateProfessionalCommand("Dra. Ana", "ana@clinica.local", null, "Veterinaria", null, "Clínica", null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.TenantId.Should().Be(tenantId);
        result.Value.Name.Should().Be("Dra. Ana");
        repository.CreatedTenantId.Should().Be(tenantId);
    }

    [Fact]
    public async Task Create_ShouldReturnForbiddenWithoutActiveTenant()
    {
        var handler = new CreateProfessionalHandler(new StubProfessionalRepository(), new StubTenantContext(null));

        var result = await handler.Handle(new CreateProfessionalCommand("Ana", null, null, "Veterinaria", null, null, null), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Forbidden);
    }

    [Fact]
    public async Task Create_ShouldReturnConflictWhenEmailAlreadyExistsInTenant()
    {
        var tenantId = Guid.NewGuid();
        var repository = new StubProfessionalRepository { EmailExists = true };
        var handler = new CreateProfessionalHandler(repository, new StubTenantContext(tenantId));

        var result = await handler.Handle(new CreateProfessionalCommand("Ana", "ana@clinica.local", null, "Veterinaria", null, null, null), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Conflict);
    }

    [Fact]
    public async Task SetActive_ShouldPersistInactivationForTheActiveTenant()
    {
        var tenantId = Guid.NewGuid();
        var professional = CreateRecord(tenantId, true);
        var repository = new StubProfessionalRepository { Record = professional };
        var handler = new SetProfessionalActiveHandler(repository, new StubTenantContext(tenantId));

        var result = await handler.Handle(new SetProfessionalActiveCommand(professional.Id, false), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Active.Should().BeFalse();
        repository.LastActiveValue.Should().BeFalse();
    }

    private static ProfessionalRecord CreateRecord(Guid tenantId, bool active) => new(Guid.NewGuid(), tenantId, null, "Ana", "ana@clinica.local", null, "Veterinaria", null, null, active, DateTime.UtcNow, null, active ? null : DateTime.UtcNow);

    private sealed class StubTenantContext(Guid? tenantId) : ITenantContext
    {
        public Guid? TenantId => tenantId;
    }

    private sealed class StubProfessionalRepository : IProfessionalRepository
    {
        public bool EmailExists { get; init; }
        public Guid? CreatedTenantId { get; private set; }
        public ProfessionalRecord? Record { get; init; }
        public bool? LastActiveValue { get; private set; }

        public Task<IReadOnlyCollection<ProfessionalRecord>> ListAsync(Guid tenantId, string? search, string? role, bool? active, CancellationToken cancellationToken) => Task.FromResult<IReadOnlyCollection<ProfessionalRecord>>(Record is null ? [] : [Record]);
        public Task<ProfessionalRecord?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) => Task.FromResult(Record);
        public Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludingId, CancellationToken cancellationToken) => Task.FromResult(EmailExists);
        public Task<ProfessionalRecord> CreateAsync(Guid tenantId, CreateProfessionalCommand command, CancellationToken cancellationToken)
        {
            CreatedTenantId = tenantId;
            return Task.FromResult(new ProfessionalRecord(Guid.NewGuid(), tenantId, command.UserId, command.Name, command.Email, command.Phone, command.Role, command.ProfessionalRegistration, command.Specialty, true, DateTime.UtcNow, null, null));
        }
        public Task<ProfessionalRecord?> UpdateAsync(Guid tenantId, UpdateProfessionalCommand command, CancellationToken cancellationToken) => Task.FromResult(Record);
        public Task<ProfessionalRecord?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken cancellationToken)
        {
            LastActiveValue = active;
            return Task.FromResult(Record is null ? null : Record with { Active = active, InactivatedAtUtc = active ? null : DateTime.UtcNow });
        }
    }
}
