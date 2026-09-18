namespace VetCommission.Application.Features.Auth.Access;

public static class AccessResources
{
    public const string AppAccess = "app.access";
    public const string AdminDashboard = "admin.dashboard";
    public const string ProfessionalPortal = "professional.portal";

    public static readonly IReadOnlyCollection<string> All =
    [
        AppAccess,
        AdminDashboard,
        ProfessionalPortal
    ];
}
