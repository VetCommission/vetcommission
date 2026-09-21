using MediatR;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.Auth.Tenant;

namespace VetCommission.Application.Features.Professionals;

internal static class ProfessionalMapping
{
    public static ProfessionalDto ToDto(ProfessionalRecord record) => new(record.Id, record.TenantId, record.UserId, record.Name, record.Email, record.Phone, record.Role, record.ProfessionalRegistration, record.Specialty, record.Active, record.CreatedAtUtc, record.UpdatedAtUtc, record.InactivatedAtUtc);
}

public sealed class ListProfessionalsHandler(IProfessionalRepository repository, ITenantContext tenant) : IRequestHandler<ListProfessionalsQuery, NotificationResult<IReadOnlyCollection<ProfessionalDto>>>
{
    public async Task<NotificationResult<IReadOnlyCollection<ProfessionalDto>>> Handle(ListProfessionalsQuery request, CancellationToken cancellationToken)
    {
        if (tenant.TenantId is not Guid tenantId) return NotificationResult<IReadOnlyCollection<ProfessionalDto>>.Failure(new NotificationError(ErrorCodes.Forbidden, "Tenant ativo obrigatorio."));
        var records = await repository.ListAsync(tenantId, request.Search, request.Role, request.Active, cancellationToken);
        return NotificationResult<IReadOnlyCollection<ProfessionalDto>>.Success(records.Select(ProfessionalMapping.ToDto).ToArray());
    }
}

public sealed class GetProfessionalHandler(IProfessionalRepository repository, ITenantContext tenant) : IRequestHandler<GetProfessionalQuery, NotificationResult<ProfessionalDto>>
{
    public async Task<NotificationResult<ProfessionalDto>> Handle(GetProfessionalQuery request, CancellationToken cancellationToken)
    {
        if (tenant.TenantId is not Guid tenantId) return NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.Forbidden, "Tenant ativo obrigatorio."));
        var record = await repository.GetAsync(tenantId, request.Id, cancellationToken);
        return record is null ? NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.NotFound, "Profissional nao encontrado.")) : NotificationResult<ProfessionalDto>.Success(ProfessionalMapping.ToDto(record));
    }
}

public sealed class CreateProfessionalHandler(IProfessionalRepository repository, ITenantContext tenant) : IRequestHandler<CreateProfessionalCommand, NotificationResult<ProfessionalDto>>
{
    public async Task<NotificationResult<ProfessionalDto>> Handle(CreateProfessionalCommand request, CancellationToken cancellationToken)
    {
        if (tenant.TenantId is not Guid tenantId) return NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.Forbidden, "Tenant ativo obrigatorio."));
        if (!string.IsNullOrWhiteSpace(request.Email) && await repository.EmailExistsAsync(tenantId, request.Email, null, cancellationToken)) return NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.Conflict, "Ja existe profissional com este e-mail no tenant."));
        var record = await repository.CreateAsync(tenantId, request, cancellationToken);
        return NotificationResult<ProfessionalDto>.Success(ProfessionalMapping.ToDto(record));
    }
}

public sealed class UpdateProfessionalHandler(IProfessionalRepository repository, ITenantContext tenant) : IRequestHandler<UpdateProfessionalCommand, NotificationResult<ProfessionalDto>>
{
    public async Task<NotificationResult<ProfessionalDto>> Handle(UpdateProfessionalCommand request, CancellationToken cancellationToken)
    {
        if (tenant.TenantId is not Guid tenantId) return NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.Forbidden, "Tenant ativo obrigatorio."));
        if (!string.IsNullOrWhiteSpace(request.Email) && await repository.EmailExistsAsync(tenantId, request.Email, request.Id, cancellationToken)) return NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.Conflict, "Ja existe profissional com este e-mail no tenant."));
        var record = await repository.UpdateAsync(tenantId, request, cancellationToken);
        return record is null ? NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.NotFound, "Profissional nao encontrado.")) : NotificationResult<ProfessionalDto>.Success(ProfessionalMapping.ToDto(record));
    }
}

public sealed class SetProfessionalActiveHandler(IProfessionalRepository repository, ITenantContext tenant) : IRequestHandler<SetProfessionalActiveCommand, NotificationResult<ProfessionalDto>>
{
    public async Task<NotificationResult<ProfessionalDto>> Handle(SetProfessionalActiveCommand request, CancellationToken cancellationToken)
    {
        if (tenant.TenantId is not Guid tenantId) return NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.Forbidden, "Tenant ativo obrigatorio."));
        var record = await repository.SetActiveAsync(tenantId, request.Id, request.Active, cancellationToken);
        return record is null ? NotificationResult<ProfessionalDto>.Failure(new NotificationError(ErrorCodes.NotFound, "Profissional nao encontrado.")) : NotificationResult<ProfessionalDto>.Success(ProfessionalMapping.ToDto(record));
    }
}
