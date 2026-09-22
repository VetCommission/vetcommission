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
import { useQuery } from "@tanstack/react-query";
import { setApiClinicId, setApiTenantId } from "@/lib/api/apiClient";
import { useQueryClient } from "@tanstack/react-query";
import type { AuthTenant } from "./authTypes";
import { useAuth } from "./AuthProvider";
import { resolveActiveTenantId } from "./resolveActiveTenant";
import { clearStoredTenantId, readStoredTenantId, storeTenantId } from "./tenantStorage";
import { clearStoredClinicId, readStoredClinicId, storeClinicId } from "./clinicStorage";
import { listClinics, type Clinic } from "@/features/clinics/clinicsApi";

type TenantContextValue = {
  tenants: AuthTenant[];
  activeTenant: AuthTenant | null;
  activeTenantId: string | null;
  hasTenant: boolean;
  selectTenant: (tenantId: string) => void;
  clearTenant: () => void;
  clinics: Clinic[]; activeClinic: Clinic | null; activeClinicId: string | null; selectClinic: (id: string) => void;
};

const TenantContext = createContext<TenantContextValue | null>(null);

export function TenantProvider({ children }: PropsWithChildren) {
  const { session, isAuthenticated } = useAuth();
  const queryClient = useQueryClient();
  const tenants = useMemo(() => session?.tenants ?? [], [session?.tenants]);
  const [selectedTenantId, setSelectedTenantId] = useState<string | null>(() =>
    readStoredTenantId(),
  );
  const activeTenantId = useMemo(
    () => (isAuthenticated ? resolveActiveTenantId(tenants, selectedTenantId) : null),
    [isAuthenticated, selectedTenantId, tenants],
  );
  const clinicsQuery = useQuery({ queryKey: ["tenant", activeTenantId, "clinics", "context"], queryFn: ({signal}) => listClinics(1, 100, undefined, true, signal), enabled: Boolean(activeTenantId) });
  const clinics = useMemo(() => clinicsQuery.data?.items ?? [], [clinicsQuery.data?.items]);
  const [selectedClinicId, setSelectedClinicId] = useState<string | null>(() => readStoredClinicId());
  const activeClinicId = useMemo(() => clinics.find(x=>x.id===selectedClinicId)?.id ?? clinics[0]?.id ?? null, [clinics, selectedClinicId]);

  useEffect(() => {
    if (!isAuthenticated) {
      setApiTenantId(null);
      setApiClinicId(null);
      clearStoredTenantId();
      clearStoredClinicId();
      return;
    }

    setApiTenantId(activeTenantId);
    setApiClinicId(activeClinicId);

    if (activeTenantId) {
      storeTenantId(activeTenantId);
    } else {
      clearStoredTenantId();
      clearStoredClinicId();
      setApiClinicId(null);
    }
  }, [activeClinicId, activeTenantId, isAuthenticated]);

  const selectClinic = useCallback((id:string)=>{if(!clinics.some(x=>x.id===id))return;setSelectedClinicId(id);storeClinicId(id);setApiClinicId(id);void queryClient.cancelQueries();queryClient.removeQueries({predicate:q=>q.queryKey[0]==="tenant"&&q.queryKey[1]===activeTenantId&&q.queryKey[2]!=="clinics"});},[activeTenantId,clinics,queryClient]);

  const selectTenant = useCallback(
    (tenantId: string) => {
      if (!tenants.some((tenant) => tenant.tenantId === tenantId)) {
        return;
      }

      setSelectedTenantId(tenantId);
      void queryClient.cancelQueries();
      queryClient.removeQueries({
        predicate: (query) => query.queryKey[0] === "tenant" && query.queryKey[1] !== tenantId,
      });
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
      clinics, activeClinic: clinics.find(x=>x.id===activeClinicId) ?? null, activeClinicId, selectClinic,
    }),
    [activeClinicId, activeTenant, activeTenantId, clearTenant, clinics, selectClinic, selectTenant, tenants],
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
