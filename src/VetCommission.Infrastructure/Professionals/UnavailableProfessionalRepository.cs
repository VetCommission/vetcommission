using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.Professionals;

namespace VetCommission.Infrastructure.Professionals;

public sealed class UnavailableProfessionalRepository : IProfessionalRepository
{
    private static InvalidOperationException MissingConnection() =>
        new("ConnectionStrings:DefaultConnection nao foi configurada. Configure a conexao com o PostgreSQL para usar profissionais.");

    public Task<PagedResult<ProfessionalRecord>> ListAsync(Guid tenantId, int page, int pageSize, string? search, string? role, bool? active, CancellationToken cancellationToken) => Task.FromException<PagedResult<ProfessionalRecord>>(MissingConnection());
    public Task<ProfessionalRecord?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) => Task.FromException<ProfessionalRecord?>(MissingConnection());
    public Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludingId, CancellationToken cancellationToken) => Task.FromException<bool>(MissingConnection());
    public Task<ProfessionalRecord> CreateAsync(Guid tenantId, CreateProfessionalCommand command, CancellationToken cancellationToken) => Task.FromException<ProfessionalRecord>(MissingConnection());
    public Task<ProfessionalRecord?> UpdateAsync(Guid tenantId, UpdateProfessionalCommand command, CancellationToken cancellationToken) => Task.FromException<ProfessionalRecord?>(MissingConnection());
    public Task<ProfessionalRecord?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken cancellationToken) => Task.FromException<ProfessionalRecord?>(MissingConnection());
}
