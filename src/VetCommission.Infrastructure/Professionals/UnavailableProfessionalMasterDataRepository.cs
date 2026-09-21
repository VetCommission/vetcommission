using VetCommission.Application.Features.ProfessionalMasterData;

namespace VetCommission.Infrastructure.Professionals;

public sealed class UnavailableProfessionalMasterDataRepository : IProfessionalMasterDataRepository
{
    public Task<ProfessionalMasterDataDto> ListAsync(Guid tenantId, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("Banco de dados nao configurado.");
}
