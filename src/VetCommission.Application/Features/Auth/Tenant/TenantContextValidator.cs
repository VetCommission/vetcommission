using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;

namespace VetCommission.Application.Features.Auth.Tenant;

public sealed class TenantContextValidator(IAuthRepository authRepository) : ITenantContextValidator
{
    public async Task<NotificationResult<Guid>> ValidateAsync(
        Guid userId,
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        var hasTenant = await authRepository.UserHasActiveTenantAsync(userId, tenantId, cancellationToken);

        if (!hasTenant)
        {
            return NotificationResult<Guid>.Failure(
                new NotificationError(ErrorCodes.Forbidden, "Usuário não possui vínculo ativo com o tenant informado."));
        }

        return NotificationResult<Guid>.Success(tenantId);
    }
}
