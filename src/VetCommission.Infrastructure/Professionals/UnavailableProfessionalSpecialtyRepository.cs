using VetCommission.Application.Features.ProfessionalSpecialties;
using VetCommission.Application.Common.Results;
namespace VetCommission.Infrastructure.Professionals;
public sealed class UnavailableProfessionalSpecialtyRepository : IProfessionalSpecialtyRepository
{
    public Task<PagedResult<ProfessionalSpecialtyDto>> ListAsync(Guid tenantId, int page, int pageSize, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
    public Task<ProfessionalSpecialtyDto?> SaveAsync(Guid tenantId, SaveProfessionalSpecialtyCommand command, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
    public Task<ProfessionalSpecialtyDto?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
}
