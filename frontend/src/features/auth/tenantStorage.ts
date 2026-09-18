const tenantKey = "vetcommission.auth.activeTenantId";

function canUseStorage() {
  return typeof window !== "undefined" && Boolean(window.localStorage);
}

export function readStoredTenantId() {
  if (!canUseStorage()) {
    return null;
  }

  return window.localStorage.getItem(tenantKey);
}

export function storeTenantId(tenantId: string) {
  if (!canUseStorage()) {
    return;
  }

  window.localStorage.setItem(tenantKey, tenantId);
}

export function clearStoredTenantId() {
  if (!canUseStorage()) {
    return;
  }

  window.localStorage.removeItem(tenantKey);
}
