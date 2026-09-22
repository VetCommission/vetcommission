import { apiClient } from "@/lib/api/apiClient"; import { endpoints } from "@/lib/api/endpoints"; import type { PaginatedResponse } from "@/types/pagination";
export type ProcedureCategory={id:string;tenantId:string;clinicId:string;name:string;description:string|null;active:boolean};
export type ProcedureCategoryInput={name:string;description?:string};
export async function listProcedureCategories(page=1,pageSize=20,search?:string,signal?:AbortSignal){return (await apiClient.get<PaginatedResponse<ProcedureCategory>>(endpoints.procedureCategories,{params:{page,pageSize,busca:search||undefined},signal})).data;}
export async function getProcedureCategory(id:string,signal?:AbortSignal){return (await apiClient.get<ProcedureCategory>(`${endpoints.procedureCategories}/${id}`,{signal})).data;}
export async function saveProcedureCategory(input:ProcedureCategoryInput,id?:string){return (await (id?apiClient.put(`${endpoints.procedureCategories}/${id}`,input):apiClient.post(endpoints.procedureCategories,input))).data;}
export async function setProcedureCategoryActive(id:string,active:boolean){return (await apiClient.post(`${endpoints.procedureCategories}/${id}/${active?"ativar":"inativar"}`)).data;}
