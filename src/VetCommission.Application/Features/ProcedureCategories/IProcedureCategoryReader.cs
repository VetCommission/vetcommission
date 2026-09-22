namespace VetCommission.Application.Features.ProcedureCategories;
public interface IProcedureCategoryReader { Task<ProcedureCategoryDto?> GetAsync(Guid tenantId, Guid clinicId, Guid id, CancellationToken cancellationToken); }
