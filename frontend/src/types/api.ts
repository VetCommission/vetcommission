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
