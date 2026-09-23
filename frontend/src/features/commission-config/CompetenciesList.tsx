"use client";

import { Alert, Button, Chip, Paper, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useTenant } from "@/features/auth/TenantProvider";
import { closeCompetency, listCompetencies } from "./commissionApi";
import { CrudActions } from "@/components/tables/CrudActions";

export function CompetenciesList() {
  const { activeTenantId } = useTenant();
  const router = useRouter();
  const queryClient = useQueryClient();
  const query = useQuery({ queryKey: ["tenant", activeTenantId, "competencies"], queryFn: ({ signal }) => listCompetencies(1, 100, signal), enabled: Boolean(activeTenantId) });
  const close = useMutation({ mutationFn: closeCompetency, onSuccess: () => queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "competencies"] }) });

  return <Stack spacing={3}>
    <Stack direction={{ xs: "column", sm: "row" }} spacing={2} sx={{ justifyContent: "space-between", alignItems: { sm: "center" } }}><Typography component="h1" variant="h4">Competencias</Typography><Button component={Link} href="/app/competencias/nova" variant="contained">Nova competencia</Button></Stack>
    {query.isError ? <Alert severity="error">Nao foi possivel carregar competencias.</Alert> : null}
    <Paper variant="outlined" sx={{ width: "100%", overflowX: "auto" }}><Table sx={{ width: "100%", minWidth: 760 }}>
      <TableHead><TableRow><TableCell>Referencia</TableCell><TableCell>Status</TableCell><TableCell>Fechamento</TableCell><TableCell align="right">Acoes</TableCell></TableRow></TableHead>
      <TableBody>{query.data?.items.map(item => <TableRow key={item.id} hover onDoubleClick={() => router.push(`/app/competencias/${item.id}`)} sx={{ cursor: "pointer" }}>
        <TableCell><Typography sx={{ fontWeight: 600 }}>{String(item.month).padStart(2, "0")}/{item.year}</Typography></TableCell>
        <TableCell><Chip size="small" color={item.status === "open" ? "success" : "default"} label={item.status === "open" ? "Aberta" : "Fechada"} /></TableCell>
        <TableCell>{item.closedAtUtc ? new Date(item.closedAtUtc).toLocaleString("pt-BR") : "—"}</TableCell>
        <TableCell align="right"><CrudActions><Button component={Link} href={`/app/competencias/${item.id}`}>Visualizar</Button>{item.status === "open" ? <Button disabled={close.isPending} onClick={() => close.mutate(item.id)}>Fechar</Button> : null}</CrudActions></TableCell>
      </TableRow>)}</TableBody>
    </Table></Paper>
  </Stack>;
}
