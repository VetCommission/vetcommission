"use client";

import { Alert, CircularProgress, Container } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { useParams, useSearchParams } from "next/navigation";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { useTenant } from "@/features/auth/TenantProvider";
import { CommissionRuleEditPage } from "@/features/commission-config/CommissionRuleForm";
import { listCompetencies } from "@/features/commission-config/commissionApi";

export default function Page() {
  const { id } = useParams<{ id: string }>();
  const ruleId = useSearchParams().get("ruleId") ?? undefined;
  const { activeTenantId } = useTenant();
  const query = useQuery({ queryKey: ["tenant", activeTenantId, "competencies"], queryFn: ({ signal }) => listCompetencies(1, 100, signal), enabled: Boolean(activeTenantId && id) });
  const competency = query.data?.items.find(item => item.id === id);

  return <RequireAcesso recurso={accessResources.competenciesManage}><AuthenticatedNavbar /><Container maxWidth="xl" sx={{ py: 5 }}>
    {query.isLoading ? <CircularProgress /> : query.isError ? <Alert severity="error">Nao foi possivel carregar a competencia.</Alert> : competency ? <CommissionRuleEditPage competencyId={id} ruleId={ruleId} locked={competency.status !== "open"} /> : <Alert severity="error">Competencia nao encontrada.</Alert>}
  </Container></RequireAcesso>;
}
