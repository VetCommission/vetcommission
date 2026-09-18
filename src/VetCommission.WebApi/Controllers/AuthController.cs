using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetCommission.Application.Features.Auth.Login;
using VetCommission.Application.Features.Auth.Me;
using VetCommission.WebApi.Auth;
using VetCommission.WebApi.Extensions;

namespace VetCommission.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.ToActionResult(this);
    }

    [Authorize(Policy = AuthorizationPolicyNames.AppAccess)]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCurrentUserQuery(), cancellationToken);
        return result.ToActionResult(this);
    }
}
