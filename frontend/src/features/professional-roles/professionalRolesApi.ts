import { apiClient } from "@/lib/api/apiClient";
import { endpoints } from "@/lib/api/endpoints";
import type { PaginatedResponse } from "@/types/pagination";

export type ProfessionalRole = { id: string; name: string; active: boolean };
export type ProfessionalRoleInput = { name: string };
export async function listProfessionalRoles(page = 1, pageSize = 20, signal?: AbortSignal) {
  return (
    await apiClient.get<PaginatedResponse<ProfessionalRole>>(endpoints.professionalRoles, {
      params: { page, pageSize },
      signal,
    })
  ).data;
}
export async function saveProfessionalRole(name: string, id?: string) {
  return (
    await (id
      ? apiClient.put(`${endpoints.professionalRoles}/${id}`, { name })
      : apiClient.post(endpoints.professionalRoles, { name }))
  ).data;
}
