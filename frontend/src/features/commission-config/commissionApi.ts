import { apiClient } from "@/lib/api/apiClient";
import { endpoints } from "@/lib/api/endpoints";
import type { PaginatedResponse } from "@/types/pagination";

export type Competency = { id: string; year: number; month: number; status: string; closedAtUtc: string | null };
export type Rule = { id: string; competencyId: string; procedureId: string; ruleType: string; percentage: number | null; fixedValue: number | null; active: boolean };

export async function listCompetencies(page = 1, pageSize = 20, signal?: AbortSignal) {
  return (await apiClient.get<PaginatedResponse<Competency>>(endpoints.competencies, { params: { page, pageSize }, signal })).data;
}

export async function saveCompetency(value: { year: number; month: number }, id?: string) {
  return (await apiClient.post(endpoints.competencies, { ...value, id: id ?? null })).data;
}

export async function closeCompetency(id: string) {
  return (await apiClient.post(`${endpoints.competencies}/${id}/fechar`)).data;
}

export async function listRules(id: string, signal?: AbortSignal) {
  return (await apiClient.get<Rule[]>(`${endpoints.competencies}/${id}/regras`, { signal })).data;
}

export async function saveRule(id: string, value: Omit<Rule, "id" | "competencyId" | "active">, ruleId?: string) {
  return (await apiClient.post(`${endpoints.competencies}/${id}/regras`, { ...value, id: ruleId ?? null })).data;
}
