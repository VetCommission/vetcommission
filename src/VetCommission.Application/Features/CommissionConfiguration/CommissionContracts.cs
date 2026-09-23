using MediatR;using VetCommission.Application.Common.Results;
namespace VetCommission.Application.Features.CommissionConfiguration;
public sealed record CompetencyDto(Guid Id,Guid TenantId,Guid ClinicId,short Year,short Month,string Status,DateTime? ClosedAtUtc);
public sealed record CommissionRuleDto(Guid Id,Guid CompetencyId,Guid ProcedureId,string RuleType,decimal? Percentage,decimal? FixedValue,bool Active);
public sealed record ListCompetenciesQuery(int Page=1,int PageSize=20):IRequest<NotificationResult<PagedResult<CompetencyDto>>>;
public sealed record SaveCompetencyCommand(Guid? Id,short Year,short Month):IRequest<NotificationResult<CompetencyDto>>;
public sealed record SetCompetencyClosedCommand(Guid Id,bool Closed):IRequest<NotificationResult<CompetencyDto>>;
public sealed record ListCommissionRulesQuery(Guid CompetencyId):IRequest<NotificationResult<IReadOnlyList<CommissionRuleDto>>>;
public sealed record SaveCommissionRuleCommand(Guid? Id,Guid CompetencyId,Guid ProcedureId,string RuleType,decimal? Percentage,decimal? FixedValue):IRequest<NotificationResult<CommissionRuleDto>>;
public interface ICommissionConfigurationRepository
{
    Task<PagedResult<CompetencyDto>> ListCompetenciesAsync(Guid t,Guid c,int p,int s,CancellationToken ct);
    Task<CompetencyDto?> GetCompetencyAsync(Guid t,Guid c,Guid id,CancellationToken ct);
    Task<CompetencyDto?> SaveCompetencyAsync(Guid t,Guid c,SaveCompetencyCommand v,CancellationToken ct);
    Task<CompetencyDto?> SetCompetencyClosedAsync(Guid t,Guid c,Guid id,bool closed,CancellationToken ct);
    Task<IReadOnlyList<CommissionRuleDto>> ListRulesAsync(Guid t,Guid c,Guid competency,CancellationToken ct);
    Task<bool> ProcedureExistsAsync(Guid t,Guid c,Guid procedureId,CancellationToken ct);
    Task<CommissionRuleDto?> SaveRuleAsync(Guid t,Guid c,SaveCommissionRuleCommand v,CancellationToken ct);
}
