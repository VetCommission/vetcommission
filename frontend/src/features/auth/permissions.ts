import type { AuthTenant } from "@/types/api";
import type { AccessResource } from "./accessResources";

export function tenantHasAccess(tenant: AuthTenant | null, resource: AccessResource) {
  return Boolean(tenant?.recursos.includes(resource));
}
