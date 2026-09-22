using Microsoft.EntityFrameworkCore;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.Auth.Tenant;
using VetCommission.Application.Features.ProfessionalSpecialties;
using VetCommission.Infrastructure.Persistence.Generated;
using VetCommission.Infrastructure.Persistence.Generated.Entities;
namespace VetCommission.Infrastructure.Professionals;
public sealed class ProfessionalSpecialtyRepository(VetCommissionDbContext db, IClinicContext clinicContext) : IProfessionalSpecialtyRepository
{
 public async Task<PagedResult<ProfessionalSpecialtyDto>> ListAsync(Guid t,int p,int s,CancellationToken c){var k=await Clinic(t,c);var q=db.ProfessionalSpecialties.AsNoTracking().Where(x=>x.TenantId==t&&x.ClinicId==k);var n=await q.CountAsync(c);var i=await q.OrderBy(x=>x.Name).ThenBy(x=>x.Id).Skip((p-1)*s).Take(s).Select(x=>new ProfessionalSpecialtyDto(x.Id,x.Name,x.Active)).ToArrayAsync(c);return new(i,p,s,n);}
 public async Task<ProfessionalSpecialtyDto?> SaveAsync(Guid t,SaveProfessionalSpecialtyCommand c,CancellationToken ct){var k=await Clinic(t,ct);var e=c.Id is Guid id?await db.ProfessionalSpecialties.SingleOrDefaultAsync(x=>x.TenantId==t&&x.ClinicId==k&&x.Id==id,ct):new ProfessionalSpecialty{Id=Guid.NewGuid(),TenantId=t,ClinicId=k,Active=true,CreatedAtUtc=DateTime.UtcNow};if(e is null)return null;e.Name=c.Name.Trim();e.UpdatedAtUtc=DateTime.UtcNow;if(db.Entry(e).State==EntityState.Detached)db.ProfessionalSpecialties.Add(e);await db.SaveChangesAsync(ct);return new(e.Id,e.Name,e.Active);}
 public async Task<ProfessionalSpecialtyDto?> SetActiveAsync(Guid t,Guid id,bool active,CancellationToken c){var k=await Clinic(t,c);var e=await db.ProfessionalSpecialties.SingleOrDefaultAsync(x=>x.TenantId==t&&x.ClinicId==k&&x.Id==id,c);if(e is null)return null;e.Active=active;e.UpdatedAtUtc=DateTime.UtcNow;await db.SaveChangesAsync(c);return new(e.Id,e.Name,e.Active);}
 private async Task<Guid> Clinic(Guid t,CancellationToken c)=>clinicContext.ClinicId is Guid id&&await db.Clinics.AnyAsync(x=>x.TenantId==t&&x.Id==id&&x.Active,c)?id:await db.Clinics.Where(x=>x.TenantId==t&&x.Active).OrderBy(x=>x.Name).ThenBy(x=>x.Id).Select(x=>x.Id).FirstAsync(c);
}
