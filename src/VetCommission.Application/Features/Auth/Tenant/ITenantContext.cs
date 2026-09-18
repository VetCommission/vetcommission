namespace VetCommission.Application.Features.Auth.Tenant;

public interface ITenantContext
{
    Guid? TenantId { get; }
}
