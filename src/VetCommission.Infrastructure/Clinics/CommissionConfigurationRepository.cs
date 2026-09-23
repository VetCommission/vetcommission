using Microsoft.EntityFrameworkCore;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.CommissionConfiguration;
using VetCommission.Infrastructure.Persistence.Generated;
using VetCommission.Infrastructure.Persistence.Generated.Entities;

namespace VetCommission.Infrastructure.Clinics;

public sealed class CommissionConfigurationRepository(VetCommissionDbContext db) : ICommissionConfigurationRepository
{
    private static CompetencyDto ToDto(Competency value) =>
        new(value.Id, value.TenantId, value.ClinicId, value.Year, value.Month, value.Status, value.ClosedAtUtc);

    private static CommissionRuleDto ToDto(CommissionRule value) =>
        new(value.Id, value.CompetencyId, value.ProcedureId, value.RuleType, value.Percentage, value.FixedValue, value.Active);

    public async Task<PagedResult<CompetencyDto>> ListCompetenciesAsync(Guid tenantId, Guid clinicId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = db.Competencies.AsNoTracking().Where(x => x.TenantId == tenantId && x.ClinicId == clinicId);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
            .Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync(cancellationToken);
        return new PagedResult<CompetencyDto>(items.Select(ToDto).ToArray(), page, pageSize, total);
    }

    public async Task<CompetencyDto?> GetCompetencyAsync(Guid tenantId, Guid clinicId, Guid id, CancellationToken cancellationToken)
    {
        var value = await db.Competencies.AsNoTracking()
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Id == id, cancellationToken);
        return value is null ? null : ToDto(value);
    }

    public async Task<CompetencyDto?> SaveCompetencyAsync(Guid tenantId, Guid clinicId, SaveCompetencyCommand value, CancellationToken cancellationToken)
    {
        var entity = value.Id is Guid id
            ? await db.Competencies.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Id == id, cancellationToken)
            : new Competency
            {
                Id = Guid.NewGuid(), TenantId = tenantId, ClinicId = clinicId,
                Status = "open", OpenedAtUtc = DateTime.UtcNow
            };

        if (entity is null || entity.Status != "open")
        {
            return null;
        }

        entity.Year = value.Year;
        entity.Month = value.Month;
        if (db.Entry(entity).State == EntityState.Detached)
        {
            db.Competencies.Add(entity);
        }

        await db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public async Task<CompetencyDto?> SetCompetencyClosedAsync(Guid tenantId, Guid clinicId, Guid id, bool closed, CancellationToken cancellationToken)
    {
        var entity = await db.Competencies.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Id == id, cancellationToken);
        if (entity is null || !closed)
        {
            return null;
        }

        entity.Status = closed ? "closed" : "open";
        entity.ClosedAtUtc = closed ? DateTime.UtcNow : null;
        await db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public async Task<IReadOnlyList<CommissionRuleDto>> ListRulesAsync(Guid tenantId, Guid clinicId, Guid competencyId, CancellationToken cancellationToken)
    {
        var items = await db.CommissionRules.AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.CompetencyId == competencyId)
            .OrderBy(x => x.ProcedureId).ToArrayAsync(cancellationToken);
        return items.Select(ToDto).ToArray();
    }

    public Task<bool> ProcedureExistsAsync(Guid tenantId, Guid clinicId, Guid procedureId, CancellationToken cancellationToken) =>
        db.Procedures.AsNoTracking().AnyAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Id == procedureId, cancellationToken);

    public async Task<CommissionRuleDto?> SaveRuleAsync(Guid tenantId, Guid clinicId, SaveCommissionRuleCommand value, CancellationToken cancellationToken)
    {
        var competency = await db.Competencies.AsNoTracking()
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.Id == value.CompetencyId, cancellationToken);
        if (competency is null || competency.Status != "open")
        {
            return null;
        }

        var entity = value.Id is Guid id
            ? await db.CommissionRules.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClinicId == clinicId && x.CompetencyId == value.CompetencyId && x.Id == id, cancellationToken)
            : new CommissionRule
            {
                Id = Guid.NewGuid(), TenantId = tenantId, ClinicId = clinicId,
                CompetencyId = value.CompetencyId, ProcedureId = value.ProcedureId,
                Active = true, CreatedAtUtc = DateTime.UtcNow
            };

        if (entity is null)
        {
            return null;
        }

        entity.ProcedureId = value.ProcedureId;
        entity.RuleType = value.RuleType;
        entity.Percentage = value.Percentage;
        entity.FixedValue = value.FixedValue;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        if (db.Entry(entity).State == EntityState.Detached)
        {
            db.CommissionRules.Add(entity);
        }

        await db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }
}
