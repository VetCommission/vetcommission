"use client";

import { Alert, Button, Chip, CircularProgress, Container, Paper, Stack, Typography } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import Link from "next/link";
import { useParams } from "next/navigation";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { useTenant } from "@/features/auth/TenantProvider";
import { listCompetencies } from "@/features/commission-config/commissionApi";
import { CommissionRulesList } from "@/features/commission-config/CommissionRulesList";

export default function Page() {
  const { id } = useParams<{ id: string }>();
  const { activeTenantId } = useTenant();
  const query = useQuery({ queryKey: ["tenant", activeTenantId, "competencies"], queryFn: ({ signal }) => listCompetencies(1, 100, signal), enabled: Boolean(activeTenantId && id) });
  const competency = query.data?.items.find(item => item.id === id);

  return <RequireAcesso recurso={accessResources.competenciesManage}><AuthenticatedNavbar /><Container maxWidth="xl" sx={{ py: 5 }}>
    {query.isLoading ? <CircularProgress /> : query.isError ? <Stack spacing={2}><Alert severity="error">Nao foi possivel carregar a competencia.</Alert><Button component={Link} href="/app/competencias">Voltar</Button></Stack> : competency ? <Stack spacing={3}>
      <Paper variant="outlined" sx={{ p: { xs: 3, md: 4 } }}><Typography component="h1" variant="h4">Competência {String(competency.month).padStart(2, "0")}/{competency.year}</Typography><Stack direction="row" spacing={1} sx={{ alignItems: "center", mt: 1 }}><Chip size="small" color={competency.status === "open" ? "success" : "default"} label={competency.status === "open" ? "Aberta" : "Fechada"} /><Typography color="text.secondary">{competency.status === "open" ? "Aberta para configuração." : "Fechada para alterações."}</Typography></Stack></Paper>
      <CommissionRulesList competencyId={id} locked={competency.status !== "open"} />
    </Stack> : <Stack spacing={2}><Alert severity="error">Competencia nao encontrada.</Alert><Button component={Link} href="/app/competencias">Voltar</Button></Stack>}
  </Container></RequireAcesso>;
}
