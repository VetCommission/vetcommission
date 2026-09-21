using Microsoft.EntityFrameworkCore; using VetCommission.Application.Features.ProfessionalSpecialties; using VetCommission.Infrastructure.Persistence.Generated; using VetCommission.Infrastructure.Persistence.Generated.Entities;
namespace VetCommission.Infrastructure.Professionals;
public sealed class ProfessionalSpecialtyRepository(VetCommissionDbContext db) : IProfessionalSpecialtyRepository
{
 public async Task<IReadOnlyCollection<ProfessionalSpecialtyDto>> ListAsync(Guid t,CancellationToken c)=>await db.ProfessionalSpecialties.AsNoTracking().Where(x=>x.TenantId==t).OrderBy(x=>x.Name).Select(x=>new ProfessionalSpecialtyDto(x.Id,x.Name,x.Active)).ToArrayAsync(c);
 public async Task<ProfessionalSpecialtyDto?> SaveAsync(Guid t,SaveProfessionalSpecialtyCommand c,CancellationToken ct){var e=c.Id is Guid id?await db.ProfessionalSpecialties.SingleOrDefaultAsync(x=>x.TenantId==t&&x.Id==id,ct):new ProfessionalSpecialty{Id=Guid.NewGuid(),TenantId=t,Active=true,CreatedAtUtc=DateTime.UtcNow};if(e is null)return null;e.Name=c.Name.Trim();e.UpdatedAtUtc=DateTime.UtcNow;if(db.Entry(e).State==EntityState.Detached)db.ProfessionalSpecialties.Add(e);await db.SaveChangesAsync(ct);return new(e.Id,e.Name,e.Active);}
 public async Task<ProfessionalSpecialtyDto?> SetActiveAsync(Guid t,Guid id,bool active,CancellationToken ct){var e=await db.ProfessionalSpecialties.SingleOrDefaultAsync(x=>x.TenantId==t&&x.Id==id,ct);if(e is null)return null;e.Active=active;e.UpdatedAtUtc=DateTime.UtcNow;await db.SaveChangesAsync(ct);return new(e.Id,e.Name,e.Active);}
}
