import { apiClient } from "@/lib/api/apiClient";
import { endpoints } from "@/lib/api/endpoints";
import type { Professional, ProfessionalMasterData } from "@/types/api";

export type ProfessionalInput = {
  name: string;
  email?: string;
  phone?: string;
  role: string;
  professionalRegistration?: string;
  specialty?: string;
  userId?: string;
};

export async function listProfessionals(filters: { search?: string; role?: string; active?: boolean }) {
  const response = await apiClient.get<Professional[]>(endpoints.professionals, { params: { busca: filters.search || undefined, funcao: filters.role || undefined, ativo: filters.active } });
  return response.data;
}

export async function getProfessional(id: string) {
  const response = await apiClient.get<Professional>(`${endpoints.professionals}/${id}`);
  return response.data;
}

export async function createProfessional(input: ProfessionalInput) {
  const response = await apiClient.post<Professional>(endpoints.professionals, input);
  return response.data;
}

export async function updateProfessional(id: string, input: ProfessionalInput) {
  const response = await apiClient.put<Professional>(`${endpoints.professionals}/${id}`, input);
  return response.data;
}

export async function setProfessionalActive(id: string, active: boolean) {
  const response = await apiClient.post<Professional>(`${endpoints.professionals}/${id}/${active ? "ativar" : "inativar"}`);
  return response.data;
}

export async function getProfessionalMasterData() {
  const response = await apiClient.get<ProfessionalMasterData>(endpoints.professionalMasterData);
  return response.data;
}

export async function listProfessionalRoles() {
  const response = await apiClient.get<{ id: string; name: string; active: boolean }[]>(endpoints.professionalRoles);
  return response.data;
}

export async function listProfessionalSpecialties() {
  const response = await apiClient.get<{ id: string; name: string; active: boolean }[]>(endpoints.professionalSpecialties);
  return response.data;
}

export async function saveProfessionalRole(name: string, id?: string) {
  const response = id ? await apiClient.put(`${endpoints.professionalRoles}/${id}`, { name }) : await apiClient.post(endpoints.professionalRoles, { name });
  return response.data;
}

export async function saveProfessionalSpecialty(name: string, id?: string) {
  const response = id ? await apiClient.put(`${endpoints.professionalSpecialties}/${id}`, { name }) : await apiClient.post(endpoints.professionalSpecialties, { name });
  return response.data;
}
