import axios from "axios";
import { toApiError } from "./apiError";

const tenantHeaderName = "X-Tenant-Id";

let accessToken: string | null = null;
let activeTenantId: string | null = null;

export const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5077",
  timeout: 15_000,
  headers: {
    Accept: "application/json",
  },
});

apiClient.interceptors.request.use((config) => {
  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  } else {
    delete config.headers.Authorization;
  }

  if (activeTenantId) {
    config.headers[tenantHeaderName] = activeTenantId;
  } else {
    delete config.headers[tenantHeaderName];
  }

  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error: unknown) => Promise.reject(toApiError(error)),
);

export function setApiAccessToken(token: string | null) {
  accessToken = token;
}

export function setApiTenantId(tenantId: string | null) {
  activeTenantId = tenantId;
}

export function clearApiSession() {
  accessToken = null;
  activeTenantId = null;
}
