using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.CommissionConfiguration;

namespace VetCommission.Infrastructure.Clinics;

public sealed class UnavailableCommissionConfigurationRepository : ICommissionConfigurationRepository
{
    private static Exception Error() => new InvalidOperationException("Banco de dados não configurado.");

    public Task<PagedResult<CompetencyDto>> ListCompetenciesAsync(Guid a, Guid b, int c, int d, CancellationToken e) => throw Error();
    public Task<CompetencyDto?> GetCompetencyAsync(Guid a, Guid b, Guid c, CancellationToken d) => throw Error();
    public Task<CompetencyDto?> SaveCompetencyAsync(Guid a, Guid b, SaveCompetencyCommand c, CancellationToken d) => throw Error();
    public Task<CompetencyDto?> SetCompetencyClosedAsync(Guid a, Guid b, Guid c, bool d, CancellationToken e) => throw Error();
    public Task<IReadOnlyList<CommissionRuleDto>> ListRulesAsync(Guid a, Guid b, Guid c, CancellationToken d) => throw Error();
    public Task<bool> ProcedureExistsAsync(Guid a, Guid b, Guid c, CancellationToken d) => throw Error();
    public Task<CommissionRuleDto?> SaveRuleAsync(Guid a, Guid b, SaveCommissionRuleCommand c, CancellationToken d) => throw Error();
}
