using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetCommission.Application.Features.ProfessionalMasterData;
using VetCommission.WebApi.Auth;
using VetCommission.WebApi.Extensions;

namespace VetCommission.WebApi.Controllers;

[ApiController]
[Route("api/dados-mestres/profissionais")]
[Authorize(Policy = AuthorizationPolicyNames.ProfessionalsManage)]
public sealed class ProfessionalMasterDataController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        (await mediator.Send(new ListProfessionalMasterDataQuery(), cancellationToken)).ToActionResult(this);
}
