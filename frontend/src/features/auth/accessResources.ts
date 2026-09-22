export const accessResources = {
  appAccess: "app.access",
  adminDashboard: "admin.dashboard",
  professionalPortal: "professional.portal",
  professionalsMenu: "menu.profissionais",
  professionalsManage: "profissionais.gerenciar",
  professionalRolesManage: "funcoes-cargos.gerenciar",
  professionalSpecialtiesManage: "especialidades.gerenciar",
} as const;

export type AccessResource = (typeof accessResources)[keyof typeof accessResources];

export const allAccessResources = Object.values(accessResources);
