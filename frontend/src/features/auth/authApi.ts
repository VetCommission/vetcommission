import { apiClient } from "@/lib/api/apiClient";
import { endpoints } from "@/lib/api/endpoints";
import type { AuthSession, CurrentSession, LoginRequest } from "./authTypes";

export async function login(request: LoginRequest) {
  const response = await apiClient.post<AuthSession>(endpoints.auth.login, request);
  return response.data;
}

export async function getCurrentSession() {
  const response = await apiClient.get<CurrentSession>(endpoints.auth.me);
  return response.data;
}
