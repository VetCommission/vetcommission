import type { AuthTenant } from "@/types/api";

export function resolveActiveTenantId(tenants: AuthTenant[], preferredTenantId: string | null) {
  if (preferredTenantId && tenants.some((tenant) => tenant.tenantId === preferredTenantId)) {
    return preferredTenantId;
  }

  return tenants[0]?.tenantId ?? null;
}
