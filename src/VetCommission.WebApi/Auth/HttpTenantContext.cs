using VetCommission.Application.Features.Auth.Tenant;

namespace VetCommission.WebApi.Auth;

public sealed class HttpTenantContext(IHttpContextAccessor httpContextAccessor) : ITenantContext
{
    public const string HeaderName = "X-Tenant-Id";

    public Guid? TenantId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.Request.Headers[HeaderName].FirstOrDefault();

            return Guid.TryParse(value, out var tenantId)
                ? tenantId
                : null;
        }
    }
}
