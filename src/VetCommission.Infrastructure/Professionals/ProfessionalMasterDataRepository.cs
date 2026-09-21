using Microsoft.EntityFrameworkCore;
using VetCommission.Application.Features.ProfessionalMasterData;
using VetCommission.Infrastructure.Persistence.Generated;

namespace VetCommission.Infrastructure.Professionals;

public sealed class ProfessionalMasterDataRepository(VetCommissionDbContext db) : IProfessionalMasterDataRepository
{
    public async Task<ProfessionalMasterDataDto> ListAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var roles = await db.ProfessionalRoles.AsNoTracking().Where(x => x.TenantId == tenantId && x.Active).OrderBy(x => x.Name).Select(x => new MasterDataItemDto(x.Id, x.Name, x.Active)).ToArrayAsync(cancellationToken);
        var specialties = await db.ProfessionalSpecialties.AsNoTracking().Where(x => x.TenantId == tenantId && x.Active).OrderBy(x => x.Name).Select(x => new MasterDataItemDto(x.Id, x.Name, x.Active)).ToArrayAsync(cancellationToken);
        return new ProfessionalMasterDataDto(roles, specialties);
    }
}
