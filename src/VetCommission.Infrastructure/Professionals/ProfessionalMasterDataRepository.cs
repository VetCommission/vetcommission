using Microsoft.EntityFrameworkCore;
using VetCommission.Application.Features.ProfessionalMasterData;
using VetCommission.Infrastructure.Persistence.Generated;

namespace VetCommission.Infrastructure.Professionals;

public sealed class ProfessionalMasterDataRepository(VetCommissionDbContext db, VetCommission.Application.Features.Auth.Tenant.IClinicContext clinicContext) : IProfessionalMasterDataRepository
{
    public async Task<ProfessionalMasterDataDto> ListAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var clinicId = clinicContext.ClinicId is Guid id && await db.Clinics.AnyAsync(x => x.TenantId == tenantId && x.Id == id && x.Active, cancellationToken) ? id : await db.Clinics.Where(x => x.TenantId == tenantId && x.Active).OrderBy(x => x.Name).ThenBy(x => x.Id).Select(x => x.Id).FirstAsync(cancellationToken);
        var roles = await db.ProfessionalRoles.AsNoTracking().Where(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Active).OrderBy(x => x.Name).Select(x => new MasterDataItemDto(x.Id, x.Name, x.Active)).ToArrayAsync(cancellationToken);
        var specialties = await db.ProfessionalSpecialties.AsNoTracking().Where(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Active).OrderBy(x => x.Name).Select(x => new MasterDataItemDto(x.Id, x.Name, x.Active)).ToArrayAsync(cancellationToken);
        return new ProfessionalMasterDataDto(roles, specialties);
    }
}
