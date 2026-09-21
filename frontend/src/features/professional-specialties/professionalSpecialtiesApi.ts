import { apiClient } from "@/lib/api/apiClient";
import { endpoints } from "@/lib/api/endpoints";
import type { PaginatedResponse } from "@/types/pagination";

export type ProfessionalSpecialty = { id: string; name: string; active: boolean };
export type ProfessionalSpecialtyInput = { name: string };
export async function listProfessionalSpecialties(page = 1, pageSize = 20, signal?: AbortSignal) {
  return (
    await apiClient.get<PaginatedResponse<ProfessionalSpecialty>>(
      endpoints.professionalSpecialties,
      { params: { page, pageSize }, signal },
    )
  ).data;
}
export async function saveProfessionalSpecialty(name: string, id?: string) {
  return (
    await (id
      ? apiClient.put(`${endpoints.professionalSpecialties}/${id}`, { name })
      : apiClient.post(endpoints.professionalSpecialties, { name }))
  ).data;
}
