using Microsoft.EntityFrameworkCore;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.Professionals;
using VetCommission.Infrastructure.Persistence.Generated;
using VetCommission.Infrastructure.Persistence.Generated.Entities;

namespace VetCommission.Infrastructure.Professionals;

public sealed class ProfessionalRepository(VetCommissionDbContext db, VetCommission.Application.Features.Auth.Tenant.IClinicContext clinicContext) : IProfessionalRepository
{
    public async Task<PagedResult<ProfessionalRecord>> ListAsync(Guid tenantId, int page, int pageSize, string? search, string? role, bool? active, CancellationToken cancellationToken)
    {
        var clinicId = await ResolveClinicIdAsync(tenantId, cancellationToken);
        var query = db.Professionals.AsNoTracking().Where(x => x.TenantId == tenantId && x.ClinicId == clinicId);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.ToLower().Contains(search.Trim().ToLower()));
        if (!string.IsNullOrWhiteSpace(role)) query = query.Where(x => x.Role == role.Trim());
        if (active.HasValue) query = query.Where(x => x.Active == active.Value);
        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query.OrderBy(x => x.Name).ThenBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).Select(ToRecordExpression).ToArrayAsync(cancellationToken);
        return new(items, page, pageSize, totalItems);
    }

    public async Task<ProfessionalRecord?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) { var clinicId = await ResolveClinicIdAsync(tenantId, cancellationToken); return await db.Professionals.AsNoTracking().Where(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Id == id).Select(ToRecordExpression).SingleOrDefaultAsync(cancellationToken); }

    public Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludingId, CancellationToken cancellationToken)
    {
        var normalized = email.Trim().ToLower();
        return EmailExistsCoreAsync(tenantId, normalized, excludingId, cancellationToken);
    }

    public async Task<ProfessionalRecord> CreateAsync(Guid tenantId, CreateProfessionalCommand command, CancellationToken cancellationToken)
    {
        var clinicId = await ResolveClinicIdAsync(tenantId, cancellationToken);
        var entity = new Professional { Id = Guid.NewGuid(), TenantId = tenantId, ClinicId = clinicId, UserId = command.UserId, Name = command.Name.Trim(), Email = Normalize(command.Email), Phone = Normalize(command.Phone), Role = command.Role.Trim(), ProfessionalRegistration = Normalize(command.ProfessionalRegistration), Specialty = Normalize(command.Specialty), Active = true, CreatedAtUtc = DateTime.UtcNow };
        db.Professionals.Add(entity); await db.SaveChangesAsync(cancellationToken); return ToRecord(entity);
    }

    public async Task<ProfessionalRecord?> UpdateAsync(Guid tenantId, UpdateProfessionalCommand command, CancellationToken cancellationToken)
    {
        var clinicId = await ResolveClinicIdAsync(tenantId, cancellationToken);
        var entity = await db.Professionals.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Id == command.Id, cancellationToken);
        if (entity is null) return null;
        entity.UserId = command.UserId; entity.Name = command.Name.Trim(); entity.Email = Normalize(command.Email); entity.Phone = Normalize(command.Phone); entity.Role = command.Role.Trim(); entity.ProfessionalRegistration = Normalize(command.ProfessionalRegistration); entity.Specialty = Normalize(command.Specialty); entity.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken); return ToRecord(entity);
    }

    public async Task<ProfessionalRecord?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken cancellationToken)
    {
        var clinicId = await ResolveClinicIdAsync(tenantId, cancellationToken);
        var entity = await db.Professionals.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Id == id, cancellationToken);
        if (entity is null) return null;
        entity.Active = active; entity.InactivatedAtUtc = active ? null : DateTime.UtcNow; entity.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken); return ToRecord(entity);
    }

    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    private async Task<bool> EmailExistsCoreAsync(Guid tenantId, string normalized, Guid? excludingId, CancellationToken cancellationToken) { var clinicId = await ResolveClinicIdAsync(tenantId, cancellationToken); return await db.Professionals.AnyAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Email != null && x.Email.ToLower() == normalized && (!excludingId.HasValue || x.Id != excludingId.Value), cancellationToken); }
    private async Task<Guid> ResolveClinicIdAsync(Guid tenantId, CancellationToken cancellationToken) { var requested = clinicContext.ClinicId; if (requested.HasValue && await db.Clinics.AnyAsync(x => x.TenantId == tenantId && x.Id == requested.Value && x.Active, cancellationToken)) return requested.Value; return await db.Clinics.Where(x => x.TenantId == tenantId && x.Active).OrderBy(x => x.Name).ThenBy(x => x.Id).Select(x => x.Id).FirstAsync(cancellationToken); }
    private static ProfessionalRecord ToRecord(Professional x) => new(x.Id, x.TenantId, x.UserId, x.Name, x.Email, x.Phone, x.Role, x.ProfessionalRegistration, x.Specialty, x.Active, x.CreatedAtUtc, x.UpdatedAtUtc, x.InactivatedAtUtc);
    private static readonly System.Linq.Expressions.Expression<Func<Professional, ProfessionalRecord>> ToRecordExpression = x => new(x.Id, x.TenantId, x.UserId, x.Name, x.Email, x.Phone, x.Role, x.ProfessionalRegistration, x.Specialty, x.Active, x.CreatedAtUtc, x.UpdatedAtUtc, x.InactivatedAtUtc);
}
