import { accessResources } from "./accessResources";
import type { AuthTenant } from "./authTypes";

export function getAuthRedirectPath(activeTenant: AuthTenant | null) {
  if (activeTenant?.recursos.includes(accessResources.adminDashboard)) {
    return "/app";
  }

  if (activeTenant?.recursos.includes(accessResources.professionalPortal)) {
    return "/portal";
  }

  return "/app";
}
