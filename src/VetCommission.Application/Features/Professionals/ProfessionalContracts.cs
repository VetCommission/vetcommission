using VetCommission.Application.Common.Results;

namespace VetCommission.Application.Features.Professionals;

public sealed record ProfessionalDto(
    Guid Id, Guid TenantId, Guid? UserId, string Name, string? Email, string? Phone,
    string Role, string? ProfessionalRegistration, string? Specialty, bool Active,
    DateTime CreatedAtUtc, DateTime? UpdatedAtUtc, DateTime? InactivatedAtUtc);

public sealed record ProfessionalListQuery(string? Search, string? Role, bool? Active);
public sealed record CreateProfessionalCommand(string Name, string? Email, string? Phone, string Role, string? ProfessionalRegistration, string? Specialty, Guid? UserId) : MediatR.IRequest<NotificationResult<ProfessionalDto>>;
public sealed record UpdateProfessionalCommand(Guid Id, string Name, string? Email, string? Phone, string Role, string? ProfessionalRegistration, string? Specialty, Guid? UserId) : MediatR.IRequest<NotificationResult<ProfessionalDto>>;
public sealed record GetProfessionalQuery(Guid Id) : MediatR.IRequest<NotificationResult<ProfessionalDto>>;
public sealed record ListProfessionalsQuery(string? Search, string? Role, bool? Active) : MediatR.IRequest<NotificationResult<IReadOnlyCollection<ProfessionalDto>>>;
public sealed record SetProfessionalActiveCommand(Guid Id, bool Active) : MediatR.IRequest<NotificationResult<ProfessionalDto>>;

public sealed record ProfessionalRecord(
    Guid Id, Guid TenantId, Guid? UserId, string Name, string? Email, string? Phone,
    string Role, string? ProfessionalRegistration, string? Specialty, bool Active,
    DateTime CreatedAtUtc, DateTime? UpdatedAtUtc, DateTime? InactivatedAtUtc);

public interface IProfessionalRepository
{
    Task<IReadOnlyCollection<ProfessionalRecord>> ListAsync(Guid tenantId, string? search, string? role, bool? active, CancellationToken cancellationToken);
    Task<ProfessionalRecord?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludingId, CancellationToken cancellationToken);
    Task<ProfessionalRecord> CreateAsync(Guid tenantId, CreateProfessionalCommand command, CancellationToken cancellationToken);
    Task<ProfessionalRecord?> UpdateAsync(Guid tenantId, UpdateProfessionalCommand command, CancellationToken cancellationToken);
    Task<ProfessionalRecord?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken cancellationToken);
}
