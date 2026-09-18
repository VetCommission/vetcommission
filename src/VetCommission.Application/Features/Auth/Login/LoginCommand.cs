using MediatR;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.Auth;

namespace VetCommission.Application.Features.Auth.Login;

public sealed record LoginCommand(string Email, string Senha) : IRequest<NotificationResult<AuthSessionDto>>;
