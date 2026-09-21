import axios from "axios";
import { toApiError } from "./apiError";

const tenantHeaderName = "X-Tenant-Id";

let accessToken: string | null = null;
let activeTenantId: string | null = null;

export function getApiBaseUrl(): string | undefined {
  const configuredUrl = process.env.NEXT_PUBLIC_API_URL?.trim();

  if (configuredUrl) {
    return configuredUrl;
  }

  if (process.env.NODE_ENV !== "production") {
    return "http://localhost:5077";
  }

  return undefined;
}

export const apiClient = axios.create({
  baseURL: getApiBaseUrl(),
  timeout: 15_000,
  headers: {
    Accept: "application/json",
  },
});

apiClient.interceptors.request.use((config) => {
  if (!getApiBaseUrl()) {
    return Promise.reject(new Error("NEXT_PUBLIC_API_URL deve ser configurada em produção."));
  }

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
