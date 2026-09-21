"use client";

import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type PropsWithChildren,
} from "react";
import { setApiTenantId } from "@/lib/api/apiClient";
import { useQueryClient } from "@tanstack/react-query";
import type { AuthTenant } from "@/types/api";
import { useAuth } from "./AuthProvider";
import { resolveActiveTenantId } from "./resolveActiveTenant";
import { clearStoredTenantId, readStoredTenantId, storeTenantId } from "./tenantStorage";

type TenantContextValue = {
  tenants: AuthTenant[];
  activeTenant: AuthTenant | null;
  activeTenantId: string | null;
  hasTenant: boolean;
  selectTenant: (tenantId: string) => void;
  clearTenant: () => void;
};

const TenantContext = createContext<TenantContextValue | null>(null);

export function TenantProvider({ children }: PropsWithChildren) {
  const { session, isAuthenticated } = useAuth();
  const queryClient = useQueryClient();
  const tenants = useMemo(() => session?.tenants ?? [], [session?.tenants]);
  const [selectedTenantId, setSelectedTenantId] = useState<string | null>(() => readStoredTenantId());
  const activeTenantId = useMemo(
    () => (isAuthenticated ? resolveActiveTenantId(tenants, selectedTenantId) : null),
    [isAuthenticated, selectedTenantId, tenants],
  );

  useEffect(() => {
    if (!isAuthenticated) {
      setApiTenantId(null);
      clearStoredTenantId();
      return;
    }

    setApiTenantId(activeTenantId);

    if (activeTenantId) {
      storeTenantId(activeTenantId);
    } else {
      clearStoredTenantId();
    }
  }, [activeTenantId, isAuthenticated]);

  const selectTenant = useCallback(
    (tenantId: string) => {
      if (!tenants.some((tenant) => tenant.tenantId === tenantId)) {
        return;
      }

      setSelectedTenantId(tenantId);
      void queryClient.cancelQueries();
      queryClient.removeQueries({ predicate: (query) => query.queryKey[0] === "tenant" && query.queryKey[1] !== tenantId });
      setApiTenantId(tenantId);
      storeTenantId(tenantId);
    },
    [queryClient, tenants],
  );

  const clearTenant = useCallback(() => {
    setSelectedTenantId(null);
    setApiTenantId(null);
    clearStoredTenantId();
  }, []);

  const activeTenant = useMemo(
    () => tenants.find((tenant) => tenant.tenantId === activeTenantId) ?? null,
    [activeTenantId, tenants],
  );

  const value = useMemo<TenantContextValue>(
    () => ({
      tenants,
      activeTenant,
      activeTenantId,
      hasTenant: Boolean(activeTenant),
      selectTenant,
      clearTenant,
    }),
    [activeTenant, activeTenantId, clearTenant, selectTenant, tenants],
  );

  return <TenantContext.Provider value={value}>{children}</TenantContext.Provider>;
}

export function useTenant() {
  const context = useContext(TenantContext);

  if (!context) {
    throw new Error("useTenant deve ser usado dentro de TenantProvider.");
  }

  return context;
}
