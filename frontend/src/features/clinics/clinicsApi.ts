import { apiClient } from "@/lib/api/apiClient";
import { endpoints } from "@/lib/api/endpoints";
import type { PaginatedResponse } from "@/types/pagination";
export type Clinic = { id:string; tenantId:string; name:string; legalName:string|null; document:string|null; email:string|null; phone:string|null; active:boolean; createdAtUtc:string; updatedAtUtc:string|null; inactivatedAtUtc:string|null };
export type ClinicInput = { name:string; legalName?:string; document?:string; email?:string; phone?:string };
export async function listClinics(page=1,pageSize=20,search?:string,active?:boolean,signal?:AbortSignal){return (await apiClient.get<PaginatedResponse<Clinic>>(endpoints.clinics,{params:{page,pageSize,busca:search||undefined,ativo:active},signal})).data;}
export async function getClinic(id:string,signal?:AbortSignal){return (await apiClient.get<Clinic>(`${endpoints.clinics}/${id}`,{signal})).data;}
export async function saveClinic(input:ClinicInput,id?:string){return (await (id?apiClient.put(`${endpoints.clinics}/${id}`,input):apiClient.post(endpoints.clinics,input))).data;}
export async function setClinicActive(id:string,active:boolean){return (await apiClient.post(`${endpoints.clinics}/${id}/${active?"ativar":"inativar"}`)).data;}
