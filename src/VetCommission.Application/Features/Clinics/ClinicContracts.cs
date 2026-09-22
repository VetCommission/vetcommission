using VetCommission.Application.Common.Results;
namespace VetCommission.Application.Features.Clinics;
public sealed record ClinicDto(Guid Id, Guid TenantId, string Name, string? LegalName, string? Document, string? Email, string? Phone, bool Active, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc, DateTime? InactivatedAtUtc);
public sealed record ListClinicsQuery(int Page, int PageSize, string? Search, bool? Active) : MediatR.IRequest<NotificationResult<PagedResult<ClinicDto>>>;
public sealed record GetClinicQuery(Guid Id) : MediatR.IRequest<NotificationResult<ClinicDto>>;
public sealed record SaveClinicCommand(Guid? Id, string Name, string? LegalName, string? Document, string? Email, string? Phone) : MediatR.IRequest<NotificationResult<ClinicDto>>;
public sealed record SetClinicActiveCommand(Guid Id, bool Active) : MediatR.IRequest<NotificationResult<ClinicDto>>;
public interface IClinicRepository { Task<PagedResult<ClinicDto>> ListAsync(Guid tenantId,int page,int pageSize,string? search,bool? active,CancellationToken ct); Task<ClinicDto?> GetAsync(Guid tenantId,Guid id,CancellationToken ct); Task<ClinicDto?> SaveAsync(Guid tenantId,SaveClinicCommand command,CancellationToken ct); Task<ClinicDto?> SetActiveAsync(Guid tenantId,Guid id,bool active,CancellationToken ct); }
