"use client";

import type { PropsWithChildren } from "react";
import { useAuth } from "./AuthProvider";
import { useTenant } from "./TenantProvider";
import { AuthLoadingState, MissingTenantState, UnauthorizedState } from "./AuthStates";

export function RequireAuth({ children }: PropsWithChildren) {
  const { isAuthenticated, isLoading } = useAuth();
  const { hasTenant } = useTenant();

  if (isLoading) {
    return <AuthLoadingState />;
  }

  if (!isAuthenticated) {
    return <UnauthorizedState />;
  }

  if (!hasTenant) {
    return <MissingTenantState />;
  }

  return children;
}
