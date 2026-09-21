using VetCommission.Application.Common.Results;
namespace VetCommission.Application.Features.ProfessionalRoles;
public sealed record ProfessionalRoleDto(Guid Id, string Name, bool Active);
public sealed record ListProfessionalRolesQuery : MediatR.IRequest<NotificationResult<IReadOnlyCollection<ProfessionalRoleDto>>>;
public sealed record SaveProfessionalRoleCommand(Guid? Id, string Name) : MediatR.IRequest<NotificationResult<ProfessionalRoleDto>>;
public sealed record SetProfessionalRoleActiveCommand(Guid Id, bool Active) : MediatR.IRequest<NotificationResult<ProfessionalRoleDto>>;
public interface IProfessionalRoleRepository { Task<IReadOnlyCollection<ProfessionalRoleDto>> ListAsync(Guid tenantId, CancellationToken ct); Task<ProfessionalRoleDto?> SaveAsync(Guid tenantId, SaveProfessionalRoleCommand command, CancellationToken ct); Task<ProfessionalRoleDto?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken ct); }
