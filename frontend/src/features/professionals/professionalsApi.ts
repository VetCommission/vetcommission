import { apiClient } from "@/lib/api/apiClient";
import { endpoints } from "@/lib/api/endpoints";
import type { PaginatedResponse } from "@/types/pagination";
import type { Professional, ProfessionalInput } from "./professionalTypes";

export async function listProfessionals(
  query: { page: number; pageSize: number; search?: string; role?: string; active?: boolean },
  signal?: AbortSignal,
) {
  const response = await apiClient.get<PaginatedResponse<Professional>>(endpoints.professionals, {
    params: {
      page: query.page,
      pageSize: query.pageSize,
      busca: query.search || undefined,
      funcao: query.role || undefined,
      ativo: query.active,
    },
    signal,
  });
  return response.data;
}

export async function getProfessional(id: string, signal?: AbortSignal) {
  const response = await apiClient.get<Professional>(`${endpoints.professionals}/${id}`, {
    signal,
  });
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
  const response = await apiClient.post<Professional>(
    `${endpoints.professionals}/${id}/${active ? "ativar" : "inativar"}`,
  );
  return response.data;
}
