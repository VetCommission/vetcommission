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
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { clearApiSession, setApiAccessToken } from "@/lib/api/apiClient";
import type { AuthSession, CurrentSession, LoginRequest } from "@/types/api";
import { getCurrentSession, login as loginRequest } from "./authApi";
import {
  clearStoredAccessToken,
  readStoredAccessToken,
  storeAccessToken,
} from "./authStorage";

type AuthContextValue = {
  session: CurrentSession | null;
  accessToken: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (request: LoginRequest) => Promise<AuthSession>;
  logout: () => void;
};

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: PropsWithChildren) {
  const queryClient = useQueryClient();
  const [accessToken, setAccessToken] = useState<string | null>(() => readStoredAccessToken());

  useEffect(() => {
    if (accessToken) {
      setApiAccessToken(accessToken);
    }
  }, [accessToken]);

  const currentSessionQuery = useQuery({
    queryKey: ["auth", "me"],
    queryFn: getCurrentSession,
    enabled: Boolean(accessToken),
    retry: false,
  });

  const clearSession = useCallback(() => {
    queryClient.cancelQueries();
    queryClient.clear();
    setAccessToken(null);
    clearStoredAccessToken();
    clearApiSession();
  }, [queryClient]);

  useEffect(() => {
    if (currentSessionQuery.isError) {
      clearStoredAccessToken();
      clearApiSession();
      queryClient.clear();
    }
  }, [currentSessionQuery.isError, queryClient]);

  const loginMutation = useMutation({
    mutationFn: loginRequest,
    onSuccess: (session) => {
      setAccessToken(session.accessToken);
      setApiAccessToken(session.accessToken);
      storeAccessToken(session.accessToken);
      queryClient.setQueryData<CurrentSession>(["auth", "me"], {
        usuario: session.usuario,
        tenants: session.tenants,
      });
    },
  });

  const login = useCallback(
    async (request: LoginRequest) => loginMutation.mutateAsync(request),
    [loginMutation],
  );

  const value = useMemo<AuthContextValue>(
    () => ({
      session: currentSessionQuery.data ?? null,
      accessToken,
      isAuthenticated: Boolean(accessToken && currentSessionQuery.data && !currentSessionQuery.isError),
      isLoading: currentSessionQuery.isLoading || loginMutation.isPending,
      login,
      logout: clearSession,
    }),
    [
      accessToken,
      clearSession,
      currentSessionQuery.data,
      currentSessionQuery.isLoading,
      currentSessionQuery.isError,
      login,
      loginMutation.isPending,
    ],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth deve ser usado dentro de AuthProvider.");
  }

  return context;
}
