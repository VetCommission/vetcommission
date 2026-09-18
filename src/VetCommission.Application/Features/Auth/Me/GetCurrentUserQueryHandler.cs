using MediatR;
using VetCommission.Application.Common.Auth;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;

namespace VetCommission.Application.Features.Auth.Me;

public sealed class GetCurrentUserQueryHandler(ICurrentUser currentUser, IAuthRepository authRepository)
    : IRequestHandler<GetCurrentUserQuery, NotificationResult<CurrentSessionDto>>
{
    public async Task<NotificationResult<CurrentSessionDto>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
        {
            return NotificationResult<CurrentSessionDto>.Failure(
                new NotificationError(ErrorCodes.Unauthorized, "Usuário não autenticado."));
        }

        var user = await authRepository.FindByIdAsync(currentUser.UserId.Value, cancellationToken);

        if (user is null || !user.Active)
        {
            return NotificationResult<CurrentSessionDto>.Failure(
                new NotificationError(ErrorCodes.Unauthorized, "Usuário não autenticado."));
        }

        var session = new CurrentSessionDto(
            new AuthUserDto(user.Id, user.Name, user.Email),
            user.Tenants
                .Select(tenant => new AuthTenantDto(
                    tenant.TenantId,
                    tenant.Nome,
                    tenant.GrupoAcessoId,
                    tenant.GrupoAcessoNome,
                    tenant.Recursos))
                .ToArray());

        return NotificationResult<CurrentSessionDto>.Success(session);
    }
}
