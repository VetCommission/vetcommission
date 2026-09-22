using VetCommission.Application.Common.Results;
namespace VetCommission.Application.Features.ProfessionalSpecialties;
public sealed record ProfessionalSpecialtyDto(Guid Id, string Name, bool Active);
public sealed record ListProfessionalSpecialtiesQuery(int Page, int PageSize) : MediatR.IRequest<NotificationResult<PagedResult<ProfessionalSpecialtyDto>>>;
public sealed record SaveProfessionalSpecialtyCommand(Guid? Id, string Name) : MediatR.IRequest<NotificationResult<ProfessionalSpecialtyDto>>;
public sealed record SetProfessionalSpecialtyActiveCommand(Guid Id, bool Active) : MediatR.IRequest<NotificationResult<ProfessionalSpecialtyDto>>;
public interface IProfessionalSpecialtyRepository { Task<PagedResult<ProfessionalSpecialtyDto>> ListAsync(Guid tenantId, int page, int pageSize, CancellationToken ct); Task<ProfessionalSpecialtyDto?> SaveAsync(Guid tenantId, SaveProfessionalSpecialtyCommand command, CancellationToken ct); Task<ProfessionalSpecialtyDto?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken ct); }
