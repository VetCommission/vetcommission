using VetCommission.Application.Features.ProfessionalRoles;
namespace VetCommission.Infrastructure.Professionals;
public sealed class UnavailableProfessionalRoleRepository : IProfessionalRoleRepository
{
    public Task<IReadOnlyCollection<ProfessionalRoleDto>> ListAsync(Guid tenantId, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
    public Task<ProfessionalRoleDto?> SaveAsync(Guid tenantId, SaveProfessionalRoleCommand command, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
    public Task<ProfessionalRoleDto?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
}
