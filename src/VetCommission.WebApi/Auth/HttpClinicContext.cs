using VetCommission.Application.Features.Auth.Tenant;

namespace VetCommission.WebApi.Auth;

public sealed class HttpClinicContext(IHttpContextAccessor accessor) : IClinicContext
{
    public const string HeaderName = "X-Clinic-Id";

    public Guid? ClinicId
    {
        get
        {
            var value = accessor.HttpContext?.Request.Headers[HeaderName].FirstOrDefault();
            return Guid.TryParse(value, out var clinicId) ? clinicId : null;
        }
    }
}
