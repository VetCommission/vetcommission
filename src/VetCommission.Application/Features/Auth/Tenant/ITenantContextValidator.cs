using VetCommission.Application.Common.Results;

namespace VetCommission.Application.Features.Auth.Tenant;

public interface ITenantContextValidator
{
    Task<NotificationResult<Guid>> ValidateAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken);
}
