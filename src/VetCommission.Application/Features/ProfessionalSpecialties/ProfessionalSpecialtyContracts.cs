using VetCommission.Application.Common.Results;
namespace VetCommission.Application.Features.ProfessionalSpecialties;
public sealed record ProfessionalSpecialtyDto(Guid Id, string Name, bool Active);
public sealed record ListProfessionalSpecialtiesQuery : MediatR.IRequest<NotificationResult<IReadOnlyCollection<ProfessionalSpecialtyDto>>>;
public sealed record SaveProfessionalSpecialtyCommand(Guid? Id, string Name) : MediatR.IRequest<NotificationResult<ProfessionalSpecialtyDto>>;
public sealed record SetProfessionalSpecialtyActiveCommand(Guid Id, bool Active) : MediatR.IRequest<NotificationResult<ProfessionalSpecialtyDto>>;
public interface IProfessionalSpecialtyRepository { Task<IReadOnlyCollection<ProfessionalSpecialtyDto>> ListAsync(Guid tenantId, CancellationToken ct); Task<ProfessionalSpecialtyDto?> SaveAsync(Guid tenantId, SaveProfessionalSpecialtyCommand command, CancellationToken ct); Task<ProfessionalSpecialtyDto?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken ct); }
