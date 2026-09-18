namespace VetCommission.WebApi.Auth;

public static class AuthorizationPolicyNames
{
    public const string ResourcePrefix = "resource:";
    public const string AppAccess = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.AppAccess;
    public const string AdminDashboard = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.AdminDashboard;
    public const string ProfessionalPortal = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.ProfessionalPortal;

    public static string Resource(string resource)
    {
        return $"{ResourcePrefix}{resource}";
    }
}
