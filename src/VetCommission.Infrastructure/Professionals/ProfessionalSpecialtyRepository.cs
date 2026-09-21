using Microsoft.EntityFrameworkCore; using VetCommission.Application.Features.ProfessionalSpecialties; using VetCommission.Infrastructure.Persistence.Generated; using VetCommission.Infrastructure.Persistence.Generated.Entities;
using VetCommission.Application.Common.Results;
namespace VetCommission.Infrastructure.Professionals;
public sealed class ProfessionalSpecialtyRepository(VetCommissionDbContext db) : IProfessionalSpecialtyRepository
{
 public async Task<PagedResult<ProfessionalSpecialtyDto>> ListAsync(Guid t,int page,int pageSize,CancellationToken c){var q=db.ProfessionalSpecialties.AsNoTracking().Where(x=>x.TenantId==t);var total=await q.CountAsync(c);var items=await q.OrderBy(x=>x.Name).ThenBy(x=>x.Id).Skip((page-1)*pageSize).Take(pageSize).Select(x=>new ProfessionalSpecialtyDto(x.Id,x.Name,x.Active)).ToArrayAsync(c);return new(items,page,pageSize,total);}
 public async Task<ProfessionalSpecialtyDto?> SaveAsync(Guid t,SaveProfessionalSpecialtyCommand c,CancellationToken ct){var e=c.Id is Guid id?await db.ProfessionalSpecialties.SingleOrDefaultAsync(x=>x.TenantId==t&&x.Id==id,ct):new ProfessionalSpecialty{Id=Guid.NewGuid(),TenantId=t,Active=true,CreatedAtUtc=DateTime.UtcNow};if(e is null)return null;e.Name=c.Name.Trim();e.UpdatedAtUtc=DateTime.UtcNow;if(db.Entry(e).State==EntityState.Detached)db.ProfessionalSpecialties.Add(e);await db.SaveChangesAsync(ct);return new(e.Id,e.Name,e.Active);}
 public async Task<ProfessionalSpecialtyDto?> SetActiveAsync(Guid t,Guid id,bool active,CancellationToken ct){var e=await db.ProfessionalSpecialties.SingleOrDefaultAsync(x=>x.TenantId==t&&x.Id==id,ct);if(e is null)return null;e.Active=active;e.UpdatedAtUtc=DateTime.UtcNow;await db.SaveChangesAsync(ct);return new(e.Id,e.Name,e.Active);}
}
