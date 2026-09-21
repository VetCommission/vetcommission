using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetCommission.Application.Features.Professionals;
using VetCommission.WebApi.Auth;
using VetCommission.WebApi.Extensions;

namespace VetCommission.WebApi.Controllers;

[ApiController]
[Route("api/profissionais")]
[Authorize(Policy = AuthorizationPolicyNames.ProfessionalsManage)]
public sealed class ProfessionalsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? busca, [FromQuery] string? funcao, [FromQuery] bool? ativo, CancellationToken cancellationToken) =>
        (await mediator.Send(new ListProfessionalsQuery(busca, funcao, ativo), cancellationToken)).ToActionResult(this);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken) =>
        (await mediator.Send(new GetProfessionalQuery(id), cancellationToken)).ToActionResult(this);

    [HttpPost]
    public async Task<IActionResult> Create(CreateProfessionalCommand command, CancellationToken cancellationToken) =>
        (await mediator.Send(command, cancellationToken)).ToActionResult(this);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProfessionalRequest request, CancellationToken cancellationToken) =>
        (await mediator.Send(new UpdateProfessionalCommand(id, request.Name, request.Email, request.Phone, request.Role, request.ProfessionalRegistration, request.Specialty, request.UserId), cancellationToken)).ToActionResult(this);

    [HttpPost("{id:guid}/ativar")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken) =>
        (await mediator.Send(new SetProfessionalActiveCommand(id, true), cancellationToken)).ToActionResult(this);

    [HttpPost("{id:guid}/inativar")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken) =>
        (await mediator.Send(new SetProfessionalActiveCommand(id, false), cancellationToken)).ToActionResult(this);

    public sealed record UpdateProfessionalRequest(string Name, string? Email, string? Phone, string Role, string? ProfessionalRegistration, string? Specialty, Guid? UserId);
}
