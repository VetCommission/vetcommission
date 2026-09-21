export type ApiErrorItem = {
  code: string;
  message: string;
  field?: string | null;
};

export type ApiErrorEnvelope = {
  errors: ApiErrorItem[];
};

export type AuthUser = {
  id: string;
  nome: string;
  email: string;
};

export type AuthTenant = {
  tenantId: string;
  nome: string;
  grupoAcessoId: string;
  grupoAcessoNome: string;
  recursos: string[];
};

export type AuthSession = {
  accessToken: string;
  expiraEm: string;
  usuario: AuthUser;
  tenants: AuthTenant[];
};

export type CurrentSession = {
  usuario: AuthUser;
  tenants: AuthTenant[];
};

export type LoginRequest = {
  email: string;
  senha: string;
};

export type Professional = {
  id: string;
  tenantId: string;
  userId: string | null;
  name: string;
  email: string | null;
  phone: string | null;
  role: string;
  professionalRegistration: string | null;
  specialty: string | null;
  active: boolean;
  createdAtUtc: string;
  updatedAtUtc: string | null;
  inactivatedAtUtc: string | null;
};

export type ProfessionalMasterData = {
  roles: { id: string; name: string; active: boolean }[];
  specialties: { id: string; name: string; active: boolean }[];
};
