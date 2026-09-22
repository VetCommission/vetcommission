export const endpoints = {
  health: "/health",
  auth: {
    login: "/api/auth/login",
    me: "/api/auth/me",
  },
  professionals: "/api/profissionais",
  professionalMasterData: "/api/dados-mestres/profissionais",
  professionalRoles: "/api/funcoes-cargos",
  professionalSpecialties: "/api/especialidades",
  clinics: "/api/clinicas",
  procedureCategories: "/api/categorias-procedimentos",
} as const;
