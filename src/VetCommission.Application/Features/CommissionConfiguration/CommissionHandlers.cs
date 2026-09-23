using MediatR;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.Auth.Tenant;

namespace VetCommission.Application.Features.CommissionConfiguration;

public sealed class CommissionHandlers(ICommissionConfigurationRepository repo, ITenantContext tenant, IClinicContext clinic)
    : IRequestHandler<ListCompetenciesQuery, NotificationResult<PagedResult<CompetencyDto>>>,
      IRequestHandler<SaveCompetencyCommand, NotificationResult<CompetencyDto>>,
      IRequestHandler<SetCompetencyClosedCommand, NotificationResult<CompetencyDto>>,
      IRequestHandler<ListCommissionRulesQuery, NotificationResult<IReadOnlyList<CommissionRuleDto>>>,
      IRequestHandler<SaveCommissionRuleCommand, NotificationResult<CommissionRuleDto>>
{
    public async Task<NotificationResult<PagedResult<CompetencyDto>>> Handle(ListCompetenciesQuery request, CancellationToken cancellationToken)
    {
        if (!TryGetContext(out var tenantId, out var clinicId))
        {
            return NotificationResult<PagedResult<CompetencyDto>>.Failure(ContextError());
        }

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        return NotificationResult<PagedResult<CompetencyDto>>.Success(
            await repo.ListCompetenciesAsync(tenantId, clinicId, page, pageSize, cancellationToken));
    }

    public async Task<NotificationResult<CompetencyDto>> Handle(SaveCompetencyCommand request, CancellationToken cancellationToken)
    {
        if (!TryGetContext(out var tenantId, out var clinicId))
        {
            return NotificationResult<CompetencyDto>.Failure(ContextError());
        }

        if (request.Year is < 2000 or > 2200 || request.Month is < 1 or > 12)
        {
            return NotificationResult<CompetencyDto>.Failure(
                new NotificationError(ErrorCodes.Validation, "Informe uma competência válida."));
        }

        if (request.Id is Guid id)
        {
            var current = await repo.GetCompetencyAsync(tenantId, clinicId, id, cancellationToken);
            if (current is null)
            {
                return NotFound<CompetencyDto>();
            }

            if (current.Status != "open")
            {
                return Conflict<CompetencyDto>("Competência fechada não pode ser alterada.");
            }
        }

        var saved = await repo.SaveCompetencyAsync(tenantId, clinicId, request, cancellationToken);
        return saved is null
            ? Conflict<CompetencyDto>("A competência não está aberta para alteração.")
            : NotificationResult<CompetencyDto>.Success(saved);
    }

    public async Task<NotificationResult<CompetencyDto>> Handle(SetCompetencyClosedCommand request, CancellationToken cancellationToken)
    {
        if (!TryGetContext(out var tenantId, out var clinicId))
        {
            return NotificationResult<CompetencyDto>.Failure(ContextError());
        }

        if (!request.Closed)
        {
            return Conflict<CompetencyDto>("Uma competência fechada não pode ser reaberta.");
        }

        var current = await repo.GetCompetencyAsync(tenantId, clinicId, request.Id, cancellationToken);
        if (current is null)
        {
            return NotFound<CompetencyDto>();
        }

        var closed = await repo.SetCompetencyClosedAsync(tenantId, clinicId, request.Id, true, cancellationToken);
        return closed is null
            ? Conflict<CompetencyDto>("Não foi possível fechar a competência.")
            : NotificationResult<CompetencyDto>.Success(closed);
    }

    public async Task<NotificationResult<IReadOnlyList<CommissionRuleDto>>> Handle(ListCommissionRulesQuery request, CancellationToken cancellationToken)
    {
        if (!TryGetContext(out var tenantId, out var clinicId))
        {
            return NotificationResult<IReadOnlyList<CommissionRuleDto>>.Failure(ContextError());
        }

        return NotificationResult<IReadOnlyList<CommissionRuleDto>>.Success(
            await repo.ListRulesAsync(tenantId, clinicId, request.CompetencyId, cancellationToken));
    }

    public async Task<NotificationResult<CommissionRuleDto>> Handle(SaveCommissionRuleCommand request, CancellationToken cancellationToken)
    {
        if (!TryGetContext(out var tenantId, out var clinicId))
        {
            return NotificationResult<CommissionRuleDto>.Failure(ContextError());
        }

        var ruleType = request.RuleType.Trim().ToLowerInvariant();
        if (request.ProcedureId == Guid.Empty || ruleType is not ("percentage" or "fixed"))
        {
            return ValidationRule("Informe um procedimento e um tipo de regra válido.");
        }

        if (ruleType == "percentage" && (request.Percentage is null or <= 0 or > 100 || request.FixedValue is not null))
        {
            return ValidationRule("A regra percentual exige somente um percentual entre 0 e 100.");
        }

        if (ruleType == "fixed" && (request.FixedValue is null or <= 0 || request.Percentage is not null))
        {
            return ValidationRule("A regra fixa exige somente um valor maior que zero.");
        }

        var competency = await repo.GetCompetencyAsync(tenantId, clinicId, request.CompetencyId, cancellationToken);
        if (competency is null)
        {
            return NotFound<CommissionRuleDto>();
        }

        if (competency.Status != "open")
        {
            return Conflict<CommissionRuleDto>("Competência fechada não aceita alteração de regras.");
        }

        if (!await repo.ProcedureExistsAsync(tenantId, clinicId, request.ProcedureId, cancellationToken))
        {
            return NotificationResult<CommissionRuleDto>.Failure(
                new NotificationError(ErrorCodes.NotFound, "Procedimento não encontrado na clínica ativa."));
        }

        var normalized = request with { RuleType = ruleType };
        var saved = await repo.SaveRuleAsync(tenantId, clinicId, normalized, cancellationToken);
        return saved is null
            ? Conflict<CommissionRuleDto>("A competência foi fechada antes da gravação da regra.")
            : NotificationResult<CommissionRuleDto>.Success(saved);
    }

    private bool TryGetContext(out Guid tenantId, out Guid clinicId)
    {
        tenantId = tenant.TenantId ?? Guid.Empty;
        clinicId = clinic.ClinicId ?? Guid.Empty;
        return tenantId != Guid.Empty && clinicId != Guid.Empty;
    }

    private static NotificationError ContextError() =>
        new(ErrorCodes.Forbidden, "Tenant e clínica ativos são obrigatórios.");

    private static NotificationResult<T> NotFound<T>() =>
        NotificationResult<T>.Failure(new NotificationError(ErrorCodes.NotFound, "Registro não encontrado."));

    private static NotificationResult<T> Conflict<T>(string message) =>
        NotificationResult<T>.Failure(new NotificationError(ErrorCodes.Conflict, message));

    private static NotificationResult<CommissionRuleDto> ValidationRule(string message) =>
        NotificationResult<CommissionRuleDto>.Failure(new NotificationError(ErrorCodes.Validation, message));
}
