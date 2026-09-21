using Microsoft.EntityFrameworkCore; using VetCommission.Application.Features.ProfessionalRoles; using VetCommission.Infrastructure.Persistence.Generated; using VetCommission.Infrastructure.Persistence.Generated.Entities;
namespace VetCommission.Infrastructure.Professionals;
public sealed class ProfessionalRoleRepository(VetCommissionDbContext db) : IProfessionalRoleRepository
{
 public async Task<IReadOnlyCollection<ProfessionalRoleDto>> ListAsync(Guid t,CancellationToken c)=>await db.ProfessionalRoles.AsNoTracking().Where(x=>x.TenantId==t).OrderBy(x=>x.Name).Select(x=>new ProfessionalRoleDto(x.Id,x.Name,x.Active)).ToArrayAsync(c);
 public async Task<ProfessionalRoleDto?> SaveAsync(Guid t,SaveProfessionalRoleCommand c,CancellationToken ct){var e=c.Id is Guid id?await db.ProfessionalRoles.SingleOrDefaultAsync(x=>x.TenantId==t&&x.Id==id,ct):new ProfessionalRole{Id=Guid.NewGuid(),TenantId=t,Active=true,CreatedAtUtc=DateTime.UtcNow};if(e is null)return null;e.Name=c.Name.Trim();e.UpdatedAtUtc=DateTime.UtcNow;if(e.Id==Guid.Empty)db.ProfessionalRoles.Add(e);else if(db.Entry(e).State==EntityState.Detached)db.ProfessionalRoles.Add(e);await db.SaveChangesAsync(ct);return new(e.Id,e.Name,e.Active);}
 public async Task<ProfessionalRoleDto?> SetActiveAsync(Guid t,Guid id,bool active,CancellationToken ct){var e=await db.ProfessionalRoles.SingleOrDefaultAsync(x=>x.TenantId==t&&x.Id==id,ct);if(e is null)return null;e.Active=active;e.UpdatedAtUtc=DateTime.UtcNow;await db.SaveChangesAsync(ct);return new(e.Id,e.Name,e.Active);}
}
