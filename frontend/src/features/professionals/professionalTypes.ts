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
export type ProfessionalInput = {
  name: string;
  email?: string;
  phone?: string;
  role: string;
  professionalRegistration?: string;
  specialty?: string;
  userId?: string;
};
