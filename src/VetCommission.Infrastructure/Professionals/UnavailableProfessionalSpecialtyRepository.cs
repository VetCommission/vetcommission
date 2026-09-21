using VetCommission.Application.Features.ProfessionalSpecialties;
namespace VetCommission.Infrastructure.Professionals;
public sealed class UnavailableProfessionalSpecialtyRepository : IProfessionalSpecialtyRepository
{
    public Task<IReadOnlyCollection<ProfessionalSpecialtyDto>> ListAsync(Guid tenantId, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
    public Task<ProfessionalSpecialtyDto?> SaveAsync(Guid tenantId, SaveProfessionalSpecialtyCommand command, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
    public Task<ProfessionalSpecialtyDto?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken ct) => throw new InvalidOperationException("Banco de dados nao configurado.");
}
