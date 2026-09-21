using VetCommission.Application.Features.Professionals;

namespace VetCommission.Infrastructure.Professionals;

public sealed class UnavailableProfessionalRepository : IProfessionalRepository
{
    private static InvalidOperationException MissingConnection() =>
        new("ConnectionStrings:DefaultConnection nao foi configurada. Configure a conexao com o PostgreSQL para usar profissionais.");

    public Task<IReadOnlyCollection<ProfessionalRecord>> ListAsync(Guid tenantId, string? search, string? role, bool? active, CancellationToken cancellationToken) => Task.FromException<IReadOnlyCollection<ProfessionalRecord>>(MissingConnection());
    public Task<ProfessionalRecord?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) => Task.FromException<ProfessionalRecord?>(MissingConnection());
    public Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludingId, CancellationToken cancellationToken) => Task.FromException<bool>(MissingConnection());
    public Task<ProfessionalRecord> CreateAsync(Guid tenantId, CreateProfessionalCommand command, CancellationToken cancellationToken) => Task.FromException<ProfessionalRecord>(MissingConnection());
    public Task<ProfessionalRecord?> UpdateAsync(Guid tenantId, UpdateProfessionalCommand command, CancellationToken cancellationToken) => Task.FromException<ProfessionalRecord?>(MissingConnection());
    public Task<ProfessionalRecord?> SetActiveAsync(Guid tenantId, Guid id, bool active, CancellationToken cancellationToken) => Task.FromException<ProfessionalRecord?>(MissingConnection());
}
