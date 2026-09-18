using MediatR;
using VetCommission.Application.Common.Results;

namespace VetCommission.Application.Features.Auth.Me;

public sealed record GetCurrentUserQuery : IRequest<NotificationResult<CurrentSessionDto>>;
