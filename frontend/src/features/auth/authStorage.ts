const authTokenKey = "vetcommission.auth.accessToken";

function canUseStorage() {
  return typeof window !== "undefined" && Boolean(window.localStorage);
}

export function readStoredAccessToken() {
  if (!canUseStorage()) {
    return null;
  }

  return window.localStorage.getItem(authTokenKey);
}

export function storeAccessToken(accessToken: string) {
  if (!canUseStorage()) {
    return;
  }

  window.localStorage.setItem(authTokenKey, accessToken);
}

export function clearStoredAccessToken() {
  if (!canUseStorage()) {
    return;
  }

  window.localStorage.removeItem(authTokenKey);
}
