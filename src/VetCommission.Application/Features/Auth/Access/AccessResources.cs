namespace VetCommission.Application.Features.Auth.Access;

public static class AccessResources
{
    public const string AppAccess = "app.access";
    public const string AdminDashboard = "admin.dashboard";
    public const string ProfessionalPortal = "professional.portal";
    public const string ProfessionalsMenu = "menu.profissionais";
    public const string ProfessionalsManage = "profissionais.gerenciar";
    public const string ProfessionalRolesManage = "funcoes-cargos.gerenciar";
    public const string ProfessionalSpecialtiesManage = "especialidades.gerenciar";

    public static readonly IReadOnlyCollection<string> All =
    [
        AppAccess,
        AdminDashboard,
        ProfessionalPortal,
        ProfessionalsMenu,
        ProfessionalsManage
        ,ProfessionalRolesManage
        ,ProfessionalSpecialtiesManage
    ];
}
