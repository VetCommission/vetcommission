namespace VetCommission.WebApi.Auth;

public static class AuthorizationPolicyNames
{
    public const string ResourcePrefix = "resource:";
    public const string AppAccess = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.AppAccess;
    public const string AdminDashboard = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.AdminDashboard;
    public const string ProfessionalPortal = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.ProfessionalPortal;
    public const string ProfessionalsManage = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.ProfessionalsManage;
    public const string ProfessionalRolesManage = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.ProfessionalRolesManage;
    public const string ProfessionalSpecialtiesManage = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.ProfessionalSpecialtiesManage;
    public const string ClinicsManage = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.ClinicsManage;
    public const string ProcedureCategoriesManage = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.ProcedureCategoriesManage;
    public const string ProceduresManage = ResourcePrefix + VetCommission.Application.Features.Auth.Access.AccessResources.ProceduresManage;

    public static string Resource(string resource)
    {
        return $"{ResourcePrefix}{resource}";
    }
}
