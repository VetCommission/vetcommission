using MediatR;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;

namespace VetCommission.Application.Features.Auth.Login;

public sealed class LoginCommandHandler(
    IAuthRepository authRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, NotificationResult<AuthSessionDto>>
{
    public async Task<NotificationResult<AuthSessionDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await authRepository.FindByNormalizedEmailAsync(normalizedEmail, cancellationToken);

        if (user is null || !user.Active || !passwordHasher.Verify(user.PasswordHash, request.Senha))
        {
            return NotificationResult<AuthSessionDto>.Failure(
                new NotificationError(ErrorCodes.Unauthorized, "E-mail ou senha inválidos."));
        }

        if (user.Tenants.Count == 0)
        {
            return NotificationResult<AuthSessionDto>.Failure(
                new NotificationError(ErrorCodes.Forbidden, "Usuário sem vínculo ativo com tenant."));
        }

        var token = jwtTokenService.CreateToken(user);
        var session = new AuthSessionDto(
            token.AccessToken,
            token.ExpiresAt,
            new AuthUserDto(user.Id, user.Name, user.Email),
            user.Tenants
                .Select(tenant => new AuthTenantDto(
                    tenant.TenantId,
                    tenant.Nome,
                    tenant.GrupoAcessoId,
                    tenant.GrupoAcessoNome,
                    tenant.Recursos))
                .ToArray());

        return NotificationResult<AuthSessionDto>.Success(session);
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToUpperInvariant();
    }
}
