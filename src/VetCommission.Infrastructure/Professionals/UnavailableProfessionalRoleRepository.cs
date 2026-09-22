using VetCommission.Application.Features.ProfessionalRoles;
using VetCommission.Application.Common.Results;
namespace VetCommission.Infrastructure.Professionals;
public sealed class UnavailableProfessionalRoleRepository : IProfessionalRoleRepository
{
    public Task<PagedResult<ProfessionalRoleDto>> ListAsync(Guid tenantId, int page, int pageSize, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
    public Task<ProfessionalRoleDto?> SaveAsync(Guid tenantId, SaveProfessionalRoleCommand command, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
    public Task<ProfessionalRoleDto?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
}
