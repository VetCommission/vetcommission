"use client";

import type { PropsWithChildren } from "react";
import type { AccessResource } from "./accessResources";
import { ForbiddenState } from "./AuthStates";
import { RequireAuth } from "./RequireAuth";
import { useTenant } from "./TenantProvider";
import { tenantHasAccess } from "./permissions";

type RequireAcessoProps = PropsWithChildren<{
  recurso: AccessResource;
}>;

export function RequireAcesso({ children, recurso }: RequireAcessoProps) {
  const { activeTenant } = useTenant();

  return (
    <RequireAuth>
      {tenantHasAccess(activeTenant, recurso) ? children : <ForbiddenState />}
    </RequireAuth>
  );
}
