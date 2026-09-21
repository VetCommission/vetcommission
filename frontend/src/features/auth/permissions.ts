import type { AuthTenant } from "./authTypes";
import type { AccessResource } from "./accessResources";

export function tenantHasAccess(tenant: AuthTenant | null, resource: AccessResource) {
  return Boolean(tenant?.recursos.includes(resource));
}
