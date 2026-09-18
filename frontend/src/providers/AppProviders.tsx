"use client";

import { useState, type PropsWithChildren } from "react";
import { CssBaseline, ThemeProvider } from "@mui/material";
import { AppRouterCacheProvider } from "@mui/material-nextjs/v16-appRouter";
import { QueryClientProvider } from "@tanstack/react-query";
import { AuthProvider } from "@/features/auth/AuthProvider";
import { TenantProvider } from "@/features/auth/TenantProvider";
import { createQueryClient } from "@/lib/query/queryClient";
import { theme } from "@/theme/theme";
import { ToastProvider } from "./ToastProvider";

export function AppProviders({ children }: PropsWithChildren) {
  const [queryClient] = useState(createQueryClient);

  return (
    <AppRouterCacheProvider options={{ key: "vetcommission" }}>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <ToastProvider>
            <AuthProvider>
              <TenantProvider>{children}</TenantProvider>
            </AuthProvider>
          </ToastProvider>
        </ThemeProvider>
      </QueryClientProvider>
    </AppRouterCacheProvider>
  );
}
